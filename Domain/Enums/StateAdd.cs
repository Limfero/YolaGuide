namespace YolaGuide.Domain.Enums
{
    public enum StateAdd
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

        //Уточнение пердпочтений
        GettingPreferencesCategories = 40,
        StartRefiningPreferences = 41,

        //Добавление места в план на сегодня
        GettingPlanCategory = 50,
        StartAddPlan = 51,
        GettingPlanAdress = 52,
        GettingPlanPlace = 53,

        //Поиск
        GettingStringToSearch = 60,
        StartSearch = 61,
    }
}
