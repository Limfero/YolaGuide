namespace YolaGuide.Domain.Enums
{
    public enum StateAdd
    {
        Start = 0,
        End = 1,

        //Добавление места
        GettingPlaceName = 10,
        GettingPlaceStart = 11,
        GettingPlaceDescription = 12,
        GettingPlaceImage = 13,
        GettingPlaceContactInformation = 14,
        GettingPlaceAdress = 15,
        GettingPlaceYId = 16,
        GettingPlaceCoordinates = 17,
        GettingPlaceCategories = 18,

        //Добавление категории
        GettingCategoryName = 20,
        GettingCategoryStart = 21,
        GettingCategorySubcategory = 22,

        //Добавление факта
        GettingFactName = 30,
        GettingFactStart = 31,
        GettingFactDescription = 32,

        //Уточнение пердпочтений
        GettingPreferencesCategories = 40,
        GettingPreferencesStart = 41,

        //Добавление места в план на сегодня
        GettingPlanCategory = 50,
        GettingPlanStart = 51,
        GettingPlanAdress = 52,
        GettingPlanPlace = 53
    }
}
