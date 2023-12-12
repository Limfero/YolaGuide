namespace YolaGuide.Domain.Enums
{
    public enum StateAdd
    {
        Start = 0,
        End = 1,

        //Добавление места
        GettingPlaceName = 10,
        GettingPlaceDescription = 11,
        GettingPlaceImage = 12,
        GettingPlaceContactInformation = 13,
        GettingPlaceYId = 14,
        GettingPlaceCoordinates = 15,
        GettingPlaceCategories = 16,

        //Добавление категории
        GettingCategoryName = 20,
        GettingCategorySubcategory = 21,

        //Добавление факта
        GettingFactName = 30,
        GettingFactSDescription = 31,
    }
}
