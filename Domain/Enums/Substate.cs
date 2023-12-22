namespace YolaGuide.Domain.Enums
{
    public enum Substate
    {
        Start = 0,
        End = 1,

        //Добавление места
        GettingPlaceName = 10,
        GettingPlaceDescription = 11,
        GettingPlaceImage = 12,
        GettingPlaceContactInformation = 13,
        GettingPlaceAdress = 14,
        GettingPlaceYId = 15,
        GettingPlaceCoordinates = 16,
        GettingPlaceCategories = 17,

        //Добавление категории
        GettingCategoryName = 20,
        GettingCategorySubcategory = 21,

        //Добавление факта
        GettingFactName = 30,
        GettingFactDescription = 31,

        //Добавление маршрута
        GettingRouteName = 40,
        GettingRouteDescription = 41,
        GettingRouteCost = 42,
        GettingRouteTelephone = 43,
        GettingRoutePlaces = 44,
        GettingRoutePlaceAdress = 45,

        //Уточнение пердпочтений
        GettingPreferencesCategories = 50,

        //Добавление места в план на сегодня
        GettingPlanCategory = 60,
        GettingPlanPlaceName = 61,

        //Поиск
        GettingPlaceNameSearch = 70,
        GettingPlaceAdressSearch = 71,

        //Удаление места
        GettingPlaceToDelete = 80,

        //Удаление категории
        GettingCategoryToDelete = 90,

        //Удаление факта
        GettingFactToDelete = 100,

        //Удаление маршрута
        GettingRouteToDelete = 110,

        //Все маршруты
        GettingAllRoute = 120,
        GettingAllPlaceInRoute = 121,

        //План
        GettingPlaceInPlan = 130,

        //Удаление места из плана
        GettingPlaceInPlanToDelete = 140,

        //Карточка места
        GettingSimilarPlaces = 150,
        GettingPlacesNearby = 151,
        GettingPlaceInformation = 152,
        GettingPlaceRoutes = 153,
        GettingPlaceNavigation = 154,
        GettingPlaceRoutesNavigation = 155,

        //О городе
        Information = 160,
    }
}
