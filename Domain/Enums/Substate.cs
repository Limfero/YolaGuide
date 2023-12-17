namespace YolaGuide.Domain.Enums
{
    public enum Substate
    {
        Start = 0,
        End = 1,

        //Добавление места
        GettingPlaceName = 10,
        StartAddPlace = 11,
        GettingPlaceDescription = 12,
        GettingPlaceImage = 13,
        GettingPlaceContactInformation = 14,
        GettingPlaceAdress = 15,
        GettingPlaceYId = 16,
        GettingPlaceCoordinates = 17,
        GettingPlaceCategories = 18,

        //Добавление категории
        GettingCategoryName = 20,
        StartAddCategory = 21,
        GettingCategorySubcategory = 22,

        //Добавление факта
        GettingFactName = 30,
        StartAddFact = 31,
        GettingFactDescription = 32,

        //Добавление маршрута
        GettingRouteName = 40,
        StartAddRoute = 41,
        GettingRouteDescription = 42,
        GettingRouteCost = 43,
        GettingRouteTelephone = 44,
        GettingRoutePlaces = 45,
        GettingRoutePlaceAdress = 46,

        //Уточнение пердпочтений
        GettingPreferencesCategories = 50,
        StartRefiningPreferences = 51,

        //Добавление места в план на сегодня
        GettingPlanCategory = 60,
        StartAddPlan = 61,
        GettingPlanPlaceName = 62,

        //Поиск
        GettingPlaceNameSearch = 70,
        StartSearch = 71,

        //Удаление места
        GettingPlaceToDelete = 80,
        StartDeletePlace = 81,

        //Удаление категории
        GettingCategoryToDelete = 90,
        StartDeleteCategory = 91,

        //Удаление факта
        GettingFactToDelete = 100,
        StartDeleteFact = 101,

        //Удаление маршрута
        GettingRouteToDelete = 110,
        StartDeleteRoute = 111,

        //Все маршруты
        GettingAllRoute = 120,
        StartGetAllRoute = 121,
        GettingAllPlaceInRoute = 122,
    }
}
