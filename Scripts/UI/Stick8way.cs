using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

[AddComponentMenu("Input/8-Way On-Screen Stick")]
public class EightWayOnScreenStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("UI Настройки")]
    [Tooltip("Визуальная ручка стика, которая будет перемещаться")]
    public RectTransform handle;

    [Tooltip("Максимальное расстояние, на которое может сместиться ручка (в пикселях)")]
    public float movementRange = 50f;

    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;
    public Vector2 controls;
    public Vector2 bodyPos;
    public Vector2 delta;

    // Свойство необходимое для работы OnScreenControl
    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    private Vector2 m_PointerDownPos;
    private Vector2 m_StartPos; // Начальная позиция ручки

    private void Start()
    {
        if (handle != null)
        {
            m_StartPos = handle.anchoredPosition;
        }
        else
        {
            Debug.LogError("Не назначен Handle в EightWayOnScreenStick!");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null) return;

        // Запоминаем, где коснулись экрана
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out m_PointerDownPos);

        UpdateStickPosition(eventData.position, eventData.pressEventCamera);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null) return;
        UpdateStickPosition(eventData.position, eventData.pressEventCamera);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Возвращаем ручку в центр
        if (handle != null)
        {
            handle.anchoredPosition = m_StartPos;
        }
        
        // Передаем нулевое значение в Input System
        //SendValueToControl(Vector2.zero);
        controls = Vector2.zero;

    }

    private void UpdateStickPosition(Vector2 screenPosition, Camera pressEventCamera)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform, 
            screenPosition, 
            pressEventCamera, 
            out localPoint);

        // Вектор сдвига от центра (где мы коснулись) до текущей позиции пальца
        delta = localPoint - m_PointerDownPos;

        // 1. Нормализуем направление для вычисления 8-стороннего вектора
        Vector2 direction = delta.normalized;
        Vector2 snappedDirection = SnapTo8Way(direction);

        // 2. Рассчитываем визуальное смещение ручки стика
        // Ограничиваем смещение радиусом movementRange
        float distance = Mathf.Clamp(delta.magnitude, 0f, movementRange);
        
        // Двигаем ручку только по разрешенным 8 осям
        if (handle != null)
        {
            handle.anchoredPosition = m_StartPos + (direction * distance);
        }

        // 3. Отправляем жесткое 8-стороннее значение в Input System (например: 1, -1, 0)
        // Если палец ушел слишком недалеко (мертвая зона, например 10% от радиуса), не двигаемся
        if (delta.magnitude < movementRange * 0.1f)
        {
            //SendValueToControl(Vector2.zero);
            controls = Vector2.zero;
        }
        else
        {
            //SendValueToControl(snappedDirection);
            controls = snappedDirection;
        }
    }

    // Метод, который округляет любой вектор до 8 направлений
    // private Vector2 SnapTo8Way(Vector2 rawDirection)
    // {
    //     // Углы: Вправо=0, Вверх=90, Влево=180/-180, Вниз=-90
    //     float angle = Mathf.Atan2(rawDirection.y, rawDirection.x) * Mathf.Rad2Deg;

    //     // Округляем угол до ближайшего кратного 45 градусам (360 / 8 = 45)
    //     float snappedAngle = Mathf.Round(angle / 45f) * 45f;

    //     // Конвертируем обратно в радианы
    //     float rad = snappedAngle * Mathf.Deg2Rad;

    //     // Получаем чистый вектор направления
    //     Vector2 snappedVector = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

    //     // Округляем (Mathf.Round), чтобы избавиться от погрешностей с плавающей точкой
    //     // Это гарантирует, что мы получим строгие 0, 1, или -1 по осям X и Y
    //     return new Vector2(Mathf.Round(snappedVector.x), Mathf.Round(snappedVector.y));
    // }
    private Vector2 SnapTo8Way(Vector2 rawDirection)
    {
        // Углы: Вправо=0, Вверх=90, Влево=180/-180, Вниз=-90
        float angle = Mathf.Atan2(rawDirection.y, rawDirection.x) * Mathf.Rad2Deg;

        // Округляем угол до ближайшего кратного 45 градусам
        float snappedAngle = Mathf.Round(angle / 45f) * 45f;

        // Конвертируем обратно в радианы
        float rad = snappedAngle * Mathf.Deg2Rad;

        // Получаем чистый вектор направления (функции Cos и Sin автоматически дают длину вектора = 1)
        Vector2 snappedVector = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        bodyPos = new Vector2(Mathf.Round(snappedVector.x), Mathf.Round(snappedVector.y));

        // Подчищаем микро-погрешности float (например, чтобы 0.0000001 стало ровно 0)
        snappedVector.x = Mathf.Abs(snappedVector.x) < 0.001f ? 0f : snappedVector.x;
        snappedVector.y = Mathf.Abs(snappedVector.y) < 0.001f ? 0f : snappedVector.y;

        // Возвращаем вектор. 
        // По прямым он будет (1,0), (0,1) и т.д.
        // По диагоналям будет (~0.707, ~0.707).
        return snappedVector;
    }
}
