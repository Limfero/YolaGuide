using System;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;

namespace YolaGuide.Messages
{
    public static class Answer
    {
        public static List<string> WelcomeMessage { get; set; } = new()
        { 
            "Привет, я Yola Guide! Я расскажу тебе о всех местах Йошкар-Олы.",
            "Hi, I'm Yola Guide! I will tell you about all the places in Yoshkar-Ola."
        };

        public static List<string> SelectingMenuButton { get; set; } = new()
        {
            "Можете выбрать куда отправиться в меню!",
            "You can pick where to go on the menu!"
        };

        public static List<string> Settings { get; set; } = new()
        {
            "Что желаете настроить?",
            "What do you wish to customize?"
        };

        public static List<string> SuccessfullySetUpPreferences { get; set; } = new()
        {
            "Предпочтения успешно настроены!",
            "Preferences have been successfully set up!"
        };

        public static List<string> Playbill { get; set; } = new()
        {
            "В данный момент этот раздел в разработке, но вы можете посмотреть афишу на <a href=\"https://afisha.yandex.ru/yoshkar-ola\">сайте</a>",
            "Preferences have been successfully set up!"
        };

        public static List<string> SelectAdmin { get; set; } = new()
        {
            "Куда идем, магистр?",
            "Where are we going, Master?"
        };

        public static List<string> ObjectSelection { get; set; } = new()
        {
            "Над каким объектом будем проводить операцию?",
            "What facility will we be operating on?"
        };

        public static List<string> ClarificationOfPreferences { get; set; } = new()
        {
            "Выберете категории, которые вам интересны:",
            "Choose the categories you are interested in:"
        };

        public static List<string> GettingRoute { get; set; } = new()
        {
            "Выберете маршрут:",
            "Choose a route:"
        };

        public static List<string> Added { get; set; } = new()
        {
            "Добавлено!",
            "Added!"
        };

        public static List<string> SuccessfullyAdded { get; set; } = new()
        {
            "Поздравляю! Ты смог добавить\nヽ(°□° )ノ\nКуда направимся теперь?",
            "Congratulations!!! You were able to add\nヽ(°□° )ノ\nWhere do we go now?"
        };

        public static List<string> SuccessfullyDellete { get; set; } = new()
        {
            "Успешно удалено!\nКуда направимся теперь?",
            "Successfully deleted!\nWhere do we go now?"
        };

        public static List<string> NothingToOffer { get; set; } = new()
        {
            "Нам нечего вам предложить :с",
            "We have nothing to offer you :c"
        };

        public static List<string> Loading { get; set; } = new()
        {
            "Загрузка....",
            "Loading...."
        };

        public static List<string> PlacesInPlan { get; set; } = new()
        {
            "В вашем плане на сегодня:\n",
            "In your plan for today:\n"
        };

        public static List<string> PlacesInRoute { get; set; } = new()
        {
            "Места в маршруте:",
            "Places on the itinerary:"
        };

        public static List<string> GetSimilarPlaces { get; set; } = new()
        {
            "Похожие места:",
            "Similar places:"
        };

        public static List<string> GetNearbyPlaces { get; set; } = new()
        {
            "Места рядом:",
            "Places close by:"
        };

        public static List<string> Blank { get; set; } = new()
        {
            "Ой... Не ожидали, что вы сюда зайдете...\nТут пока ничего нет, но в будущем обязательно появиться! <s>Наверное</s>",
            "Oh... We didn't expect you to come here...\nThere's nothing here yet, but there will be in the future!"
        };

        public static List<string> SuccessfulLanguageChange { get; set; } = new()
        {
            "Язык успешно сменен!",
            "The language has been successfully changed!"
        };

        public static List<string> ClarificationPreferences { get; set; } = new()
        {
            "Вы хотите выбрать категории мест, которые вас интересуют?",
            "Do you want to select the categories of places you are interested in?"
        };

        // Ошибочки
        public static List<string> WrongCommand { get; set; } = new()
        {
            "К сожалению, я ещё тупенький и не знаю этой команды",
            "Unfortunately, I'm still dumb and don't know this command yet"
        };

        public static List<string> WrongInputType { get; set; } = new()
        {
            "Красивое... Но, к сожалению, я понимаю только текст :с",
            "It's beautiful... But unfortunately, I can only understand the text :с"
        };

        public static List<string> WrongLanguage { get; set; } = new()
        {
            "Я не знаю такого языка... :с",
            "I don't know that kind of language... :с"
        };

        public static List<string> WrongInputFormat { get; set; } = new()
        {
            "Ой... Ошибочка в формате написания. Пробуй ещё раз, пожалуйста!",
            "Oops... Wrong spelling format. Try again, please!"
        };

        public static List<string> Error { get; set; } = new()
        {
            "Упс... Я такого не знаю. Попробуйте выбрать из меню выше!",
            "Oops... I don't know that one. Try selecting from the menu above!"
        };

        // Добавление места
        public static List<string> EnteringPlaceName { get; set; } = new()
        {
            "Введи имя нового места(сначала на русском, потом через две строчки на английском):",
            "Enter the name of the new place(first in Russian, then two lines later in English):"
        };

        public static List<string> EnteringPlaceDescription { get; set; } = new()
        {
            "Введи описание места(сначала на русском, потом через две строчки на английском):",
            "Enter the description of the place(first in Russian, then two lines later in English):"
        };

        public static List<string> EnteringPlaceAdress { get; set; } = new()
        {
            "Введи адрес места(сначала на русском, потом через две строчки на английском):",
            "Enter the address of the place(first in Russian, then two lines later in English):"
        };


        public static List<string> EnteringPlaceContactInformation { get; set; } = new()
        {
            "Введи контактную информацию для этого места(сначала на русском, потом через две строчки на английском) (если контактной информации нет введите \"Нет\"):",
            "Enter the contact information for this place(first in Russian, then two lines later in English) (if there is no contact information, enter \"No\"):"
        };

        public static List<string> EnteringPlaceImage { get; set; } = new()
        {
            "Отправь фотографию на обложку места:",
            "Send a picture of yourself on the cover of the place:"
        };

        public static List<string> EnteringPlaceYId { get; set; } = new()
        {
            "Введи id организации в Яндекс Картах:",
            "Enter the id of the organization in Yandex Maps:"
        };

        public static List<string> EnteringPlaceCoordinates { get; set; } = new()
        {
            "Введи координаты:",
            "Enter coordinates:"
        };

        public static List<string> EnteringPlaceCategories{ get; set; } = new()
        {
            "Осталось только выбрать категории",
            "All that's left is to choose the filters:"
        };

        // Добавление категории
        public static List<string> EnteringCategoryName { get; set; } = new()
        {
            "Введи название новой категории(сначала на русском, потом через две строчки на английском):",
            "Enter the name of the new category(first in Russian, then two lines later in English):"
        };

        public static List<string> EnteringCategorySubcategory { get; set; } = new()
        {
            "Осталось понять, будет ли эта категория подкатегорией? Если да, то выбери какой!",
            "It remains to be seen if this category will be a subcategory? If so, pick which one!"
        };

        //Добавление фатка
        public static List<string> EnteringFactName { get; set; } = new()
        {
            "Придумай название фактика(сначала на русском, потом через две строчки на английском):",
            "Think up a name for the factoid(first in Russian, then two lines later in English):"
        };

        public static List<string> EnteringFactDescription { get; set; } = new()
        {
            "Ну и давай свой факт(сначала на русском, потом через две строчки на английском):",
            "So give me your fact(first in Russian, then two lines later in English):"
        };

        public static List<string> EnteringFactImage { get; set; } = new()
        {
            "Теперь нужена фотография на обложку факта:",
            "Now we need a cover photo:"
        };

        //Добавление маршрута
        public static List<string> EnteringRouteName { get; set; } = new()
        {
            "Введи название маршрута(сначала на русском, потом через две строчки на английском):",
            "Enter the name of the route(first in Russian, then two lines later in English):"
        };

        public static List<string> EnteringRouteDescription { get; set; } = new()
        {
            "Введи описание маршрута(сначала на русском, потом через две строчки на английском):",
            "Enter a description of the route(first in Russian, then two lines later in English):"
        };

        public static List<string> EnteringRouteCost { get; set; } = new()
        {
            "Введи стоимость маршрута, если он бесплатный, то укажи 0:",
            "Enter the cost of the route, if it is free, then specify 0:"
        };

        public static List<string> EnteringRouteTelephone { get; set; } = new()
        {
            "Введи номер телефона, где можно забронировать, если телефона нет, то напиши \"Нет\":",
            "Enter the phone number where you can make a reservation, and if you don't have a phone number, write \"No\":"
        };

        public static List<string> EnteringRoutePlaces { get; set; } = new()
        {
            "Осталось выбрать места, которые есть в маршруте:",
            "All that's left is to pick the places that are on the route:"
        };

        //Добавление в план
        public static List<string> PlanIsEmpty { get; set; } = new()
        {
            "Ваш план пуст! Давайте выберем куда отправиться сегодня?",
            "Your plan is empty! Let's pick a place to go today, shall we?"
        };

        public static List<string> MakingPlan { get; set; } = new()
        {
            "Выбирайте куда хотите отправиться сегодня:",
            "Choose where you want to go today:"
        };

        public static List<string> EnteringPlanPlace { get; set; } = new()
        {
            "А теперь выберете место:",
            "Now pick a place:"
        };

        public static List<string> EnteringPlanAdress { get; set; } = new()
        {
            "И осталось выбрать адрес места:",
            "And the only thing left to do is pick an address for the place:"
        };

        // Удаление
        public static List<string> DeletePlace { get; set; } = new()
        {
            "Выберете место, которое хотите удалить:",
            "Select the place you want to delete:"
        };

        public static List<string> DeleteCategory { get; set; } = new()
        {
            "Выберете категорию, которую хотите удалить:",
            "Select the category you want to delete:"
        };


        public static List<string> DeleteFact { get; set; } = new()
        {
            "Выберете факт, который хотите удалить:",
            "Select the fact you want to delete:"
        };

        public static List<string> DeleteRoute { get; set; } = new()
        {
            "Выберете маршрут, который хотите удалить:",
            "Select the route you want to delete:"
        };

        //Поиск
        public static List<string> EnteringStringToSearch { get; set; } = new()
        {
            "Введите то, что хотите найти:",
            "Enter what you want to find:"
        };

        //О городе
        public static List<string> AboutCity { get; set; } = new()
        {
            "Город на необычную букву Й, столица Республики Марий Эл, «Европа в центре России», уютный, симпатичный, сказочный и необычный город - все это Йошкар-Ола, что в переводе с марийского значит «Красный город». А красный – это красивый. Я думаю, каждый из вас, кто приедет в нашу марийскую столицу, обязательно в этом убедится. \r\n\r\nГород Йошкар-Ола (столица Республики Марий Эл) расположен в центральной части России на расстоянии 800 км от Москвы (по карте на Восток), 540 км от Самары, 330 км от Нижнего Новгорода, Кирова и Ульяновска, 150 км от Казани, 100 км от Чебоксар и 50 км от реки Волги.",
            "The city with an unusual letter Y, the capital of the Republic of Mari El, \"Europe in the center of Russia\", cozy, cute, fairy-tale and unusual city - all this is Yoshkar-Ola, which means \"Red City\" in translation from Mari. And red means beautiful. I think each of you, who will come to our Mari capital, will definitely be convinced of it. \\The city of Yoshkar-Ola (the capital of the Mari El Republic) is located in the central part of Russia at a distance of 800 km from Moscow (on the map to the East), 540 km from Samara, 330 km from Nizhny Novgorod, Kirov and Ulyanovsk, 150 km from Kazan, 100 km from Cheboksary and 50 km from the Volga River.\r\n"
        };

        public static List<string> HowToGetThere { get; set; } = new()
        {
            "Добраться до столицы Республики Марий Эл можно несколькими путями: на поезде, на автобусе, на самолете, на личном автомобиле. Поезда Москва – Йошкар-Ола ходят ежедневно. Выехав из Москвы в 16.20, вы прибудете в Йошкар-Олу 6.52 следующего утра. Расписание автобусов и наличие рейсов до Йошкар-Олы можно узнать в справочной автовокзала вашего города по номеру 8 (8362) 45-03-05.\r\n\r\nЛичный автомобиль – наиболее удобный и мобильный способ путешествий, тем более что движение в Йошкар-Оле не напряжённое, дороги приличные, парковки бесплатные.\r\nПутешествуя на своем автомобиле, вы открываете перед собой прекрасные горизонты: во время поездки у вас будет отличная возможность свободно останавливаться там, где просит ваша душа, увидеть колорит марийской природы: просторные луга и поля, вековые леса, проникнуться атмосферой «марийской Швейцарии».\r\n\r\nНо если у вас нет личного авто, либо вы хотите отдохнуть от руля, то можно воспользоваться сайтом blablacar.ru, найти попутчиков и за небольшую сумму  от Москвы (примерная стоимость от 900 до 1400 руб.) можно добраться до столицы Республики Марий Эл.",
            "There are several ways to get to the capital of the Mari El Republic: by train, by bus, by airplane, by private car. Moscow - Yoshkar-Ola trains run daily. Leaving Moscow at 16.20, you will arrive in Yoshkar-Ola at 6.52 the next morning. You can get the bus schedule and availability of flights to Yoshkar-Ola from the bus station of your city by calling 8 (8362) 45-03-05. \\Traveling by your own car, you open wonderful horizons: during the trip you will have a great opportunity to freely stop wherever your soul asks, to see the color of Mari nature: spacious meadows and fields, century-old forests, to feel the atmosphere of \"Mari Switzerland\". \\But if you do not have a personal car, or you want to relax from driving, you can use the site blablacar.ru, find hitchhikers and for a small amount of money from Moscow (approximate cost from 900 to 1400 rubles) you can get to the capital of the Republic of Mari El.\r\n"
        };

        public static List<string> WhatToSee { get; set; } = new()
        {
            "Каждый турист, который приезжает в Йошкар-Олу, обязательно в своем списке «что увидеть и посмотреть» обозначает набережную реки Малой Кокшаги. Эта ландшафтная достопримечательность привлекает глаз туриста, ведь оказавшись в Йошкар-Оле, тебя не покидает чувство сомнения, «а туда ли я приехал? не в Европе ли я?». Набережная Брюгге напоминает бельгийский город с его пряничными домиками. На ней вы найдете скульптурную композицию супружеской пары Принца Монако и Грейс Келли, а также напротив Президентской школы памятник императрице Елизавете. На противоположном берегу Кокшаги протянулась набережная Амстердам с домами в голландском стиле. Здесь вы увидите скульптуры Ганса крысолова, Рембрандта, Пушкина с Онегиным и др.\r\n\r\nЛюбимым персонажем и «главным жителем» города для многих стал Йошкин кот. Да-да, теперь это не ругательное выражение, а настоящее имя Йошкар-Олинского кота.\r\n\r\nКот вальяжно расположился на скамейке, рядом с ним - импровизированная газета «Голос правды». Этимология именования кота восходит к эвфемизму «Ёшкин кот», где буква «Ё» заменяется на «Йо», что созвучно названию города Йошкар-Ола (в молодежной речи Йошка). Как известно, эскиз Йошкина кота был сделан с обыкновенного рыжего кота, знакомого художникам. Если вы приедете в Йошкар-Олу и не сфотографируетесь с Йошкиным котом, это будет большим упущением. И да, присев к Коту, обязательно на удачу потрите нос и не забудьте прошептать ваше заветное желание ему в ухо. Говорят, он их претворяет в жизнь. ",
            "Every tourist who comes to Yoshkar-Ola, necessarily in his list of \"what to see and see\" designates the embankment of the Malaya Kokshaga River. This landscape attraction attracts the tourist's eye, because once in Yoshkar-Ola, you can't help but feel a sense of doubt: \"Have I come to the right place? Am I not in Europe?\". The embankment of Bruges reminds a Belgian city with its gingerbread houses. On it you will find a sculptural composition of the couple of Prince of Monaco and Grace Kelly, as well as a monument to Empress Elizabeth in front of the Presidential School. On the opposite bank of the Kokshaga stretches the Amsterdam embankment with houses in the Dutch style. Here you will see sculptures of Hans the Pied Piper, Rembrandt, Pushkin with Onegin and others.\r\n\r\nThe favorite character and \"main inhabitant\" of the city for many became Yoshkin cat. Yes, yes, now it is not a swear word, but the real name of Yoshkar-Ola cat.\r\nThe cat is sitting on a bench with an improvised newspaper \"Golos Pravda\" next to him. The etymology of the cat's name goes back to the euphemism \"Yoshkin cat\", where the letter \"Yo\" is replaced by \"Yo\", which is consonant with the name of the city of Yoshkar-Ola (in youth speech Yoshka). As you know, the sketch of Yoshkin cat was made from an ordinary red cat familiar to artists. If you come to Yoshkar-Ola and do not take a picture with Yoshkin cat, it will be a big omission. And yes, when you sit down to the cat, be sure to rub his nose for good luck and don't forget to whisper your wish into his ear. They say he makes them come true."
        };

        public static List<string> WhatToSee2 { get; set; } = new()
        {
            "Оказавшись в марийской столице, вы точно не потеряетесь во времени: Йошкар-Ола – это город уникальных часов. Часы с динамической скульптурной композицией «Явление иконы Божьей Матери «Троеручица» (в народе носит название «Часы с осликом») и «Вход Господень в Иерусалим» («12 апостолов») впечатлят вас и оставят только самые лучше чувства и эмоции. Площади, на которых вы сможете понаблюдать за  выходом героев, находятся в 10 минутах езды друг от друга. Площадь Оболенского-Ноготкова и Патриаршая площадь являются новыми площадями Йошкар-Олы. \r\n\r\nПатриаршая площадь, названная так в честь Патриарха Московского и Всея Руси Алексия II, еще одна из главных достопримечательностей города. Летом здесь проводится театральный фестиваль \"Летние сезоны\": на развёрнутой под открытым небом сцене творят свое изящное действо артисты балета, играет живой оркестр, а замок с часами становится площадкой для оперных певцов.  Головное здание площади, напоминающее Юринский замок графа Шереметева, в определенные часы становится площадкой для шествия двенадцати апостолов во главе с Иисусом, восседающим на осле. А по соседству расположился Республиканский театр кукол, построенный по мотивам сказочного замка в Баварии. Здесь же на Патриаршей установлены памятник и часовня в честь святых Петра и Февронии, покровителей семьи и брака. \r\n\r\nЕсли вы уже обошли все достопримечательности в городе, выбраться за пределы города и смело отправляйтесь в путь. В этнокультурном комплексе дер. Шоруньжа Моркинского района вы сможете окунуться в быт и культуру марийского народа, попробовать марийские блюда, если повезет, даже поучаствовать в марийских обрядах и свадебных церемониях. Выбравшись на озеро «Морской глаз», вы удивитесь, насколько красива марийская природа и проникнитесь легендами и тайнами этого уникального бирюзового озера.\r\n\r\nЧтобы изучить город, вам понадобится как минимум два дня. За это время вы успеете погулять по всем красивым местам города, покататься на лодках и катамаранах по реке Малая Кокшага, попробовать марийские блюда, увидеть город с высоты птичьего полета с Колеса обозрения в Центральном парке культуры и отдыха, загадать желания, проходя по мосту влюбленных.",
            "Once in the Mari capital, you won't get lost in time: Yoshkar-Ola is a city of unique clocks. The clocks with the dynamic sculptural composition \"Apparition of the Icon of the Mother of God \"Troeruchitsa\" (popularly called \"The Clock with a Donkey\") and \"The Entry of the Lord into Jerusalem\" (\"12 Apostles\") will impress you and leave only the best feelings and emotions. The squares where you can watch the heroes come out are 10 minutes away from each other. Obolensky-Nogotkov Square and Patriarch's Square are the new squares of Yoshkar-Ola. \r\n\r\nPatriarch's Square, named so in honor of Patriarch Alexy II of Moscow and All Russia, is another of the city's main attractions. In summer, the theater festival \"Summer Seasons\" is held here: on the open-air stage ballet dancers create their elegant action, a live orchestra plays, and the castle with a clock becomes a platform for opera singers.  The main building of the square, reminiscent of Count Sheremetev's castle in Jurinsk, becomes a platform for the procession of the twelve apostles led by Jesus on a donkey at certain hours. And next door is the Republican Puppet Theater, built on the motives of a fairy-tale castle in Bavaria. There is also a monument and a chapel in honor of Saints Peter and Fevronia, the patrons of family and marriage. \r\n\r\n\r\nIf you have already bypassed all the sights in the city, get out of the city and boldly set off. In the ethno-cultural complex of Shorunja village of Morkinsky district you can plunge into the life and culture of Mari people, taste Mari dishes, if you are lucky, even take part in Mari rituals and wedding ceremonies. Once you get out to the Sea Eye Lake, you will be surprised at how beautiful the Mari nature is and you will be imbued with the legends and secrets of this unique turquoise lake.\r\n\r\nYou will need at least two days to explore the city. During this time you will have time to walk around all the beautiful places of the city, ride on boats and catamarans on the Malaya Kokshaga River, taste Mari dishes, see the city from a bird's eye view from the Ferris Wheel in the Central Park of Culture and Recreation, make wishes while passing over the bridge of lovers."
        };

        public static List<string> WhatToTryFromFood { get; set; } = new()
        {
            "Приехав в Йошкар-Олу, не забывайте, что вы оказались в столице Республики Марий Эл, которая славится не только гостеприимством, но и своей марийской культурой, языком, блюдами. Из последнего обязательно рекомендуем попробовать подкоголи (вареники с очень вкусной начинкой), коман-мелна (трехслойные блины), пӧрӧмечи, выпить пура (марийский квас) или мӱй пӱрӧ (медовуху).",
            "When you come to Yoshkar-Ola, don't forget that you are in the capital of the Mari El Republic, which is famous not only for its hospitality, but also for its Mari culture, language, and dishes. Of the latter, we definitely recommend you to try podkogoli (dumplings with a very tasty filling), komu-melna (three-layer pancakes), pӧrӧmechi, drink pura (Mari kvass) or mӱy pӱrӧ (mead).\r\n"
        };

        public static List<string> WhereToLive { get; set; } = new()
        {
            "Проблем с жильем для туристов нет. Вариантов много: гостиницы, хостелы, гостевые дома или аренда квартиры или дома, – для каждого найдется что-то по душе",
            "There are no problems with accommodation for tourists. There are many options: hotels, hostels, guest houses or renting an apartment or house - there is something for everyone"
        };

        public static List<string> WhatCanIBring { get; set; } = new()
        {
            "Из местных деликатесов можно выделить сырокопченые колбасы, йогурты и сыр из козьего молока, ликеро-водочная продукция «Йошкин кот» и \"Огни Марий Эл\", марийский мед, Иван-чай, конфеты «птичье молоко», марийское мороженое, и, конечно же, магнитики, открытки и самые лучшие впечатления. Можно прикупить какие-нибудь аутентичные штуки вроде марийских лаптей (своим плетением, по форме они отличаются от русских), марийских музыкальных инструментов, одежды с марийской вышивкой или украшения.\r\n\r\nА также можете пополнить свой словарный запас парой марийских слов. Приезжайте! Покажем! Расскажем! Научим! Пагален ӱжына!",
            "Among local delicacies one can mention raw smoked sausages, yogurt and cheese from goat's milk, liquor and vodka products \"Yoshkin Cat\" and \"Ogni Mari El\", Mari honey, Ivan-tea, candies \"bird's milk\", Mari ice cream, and, of course, magnets, postcards and the best impressions. You can buy some authentic things like Mari laptops (with their weaving, in shape they differ from Russian ones), Mari musical instruments, clothes with Mari embroidery or jewelry.\r\n\r\nAnd you can also enrich your vocabulary with a couple of Mari words. Come! We will show you! We will tell! We will teach you! Pagalen ӱzhyna!"
        };

        public static string GetPlaceInformation(Place place, Language language)
        {
            string[] name = place.Name.Split("\n\n\n");
            string[] description = place.Description.Split("\n\n\n");

            return string.Format("{0}\n{1}", $"<b>{name[(int)language]}</b>", description[(int)language]);
        }

        public static string GetFactInformation(Fact fact, Language language)
        {
            string[] name = fact.Name.Split("\n\n\n");
            string[] description = fact.Description.Split("\n\n\n");

            return string.Format("{0}\n{1}", $"<b>{name[(int)language]}</b>", description[(int)language]);
        }

        public static string GetRouteInformation(Route route, Language language)
        {
            string[] name = route.Name.Split("\n\n\n");
            string[] description = route.Description.Split("\n\n\n");
            string cost = route.Cost == 0 ? "Бесплатно" : route.Cost.ToString();
            string[] telephone = route.Telephone.Trim() == "нет" || route.Telephone.Trim() == "no" ? new[] { "Нет", "No" } : new[] { route.Telephone, route.Telephone };

            string[] messageCost = new[] { "<b>Цена</b>", "<b>Cost</b>" };
            string[] messageTelephone = new[] { "<b>Телефон</b>", "<b>Telephone</b>" };

            return string.Format("{0}\n{1}\n{2} {3}\n{4} {5}", $"<b>{name[(int)language]}</b>", description[(int)language], messageCost[(int)language], cost, messageTelephone[(int)language], telephone[(int)language]);
        }

        public static string GetPlanInformation(User user)
        {
            var placesInPlan = user.Places;
            string result = PlacesInPlan[(int)user.Language];

            for(int i = 0; i < placesInPlan.Count; i++) 
            {
                var name = placesInPlan[i].Name.Split("\n\n\n")[(int)user.Language];
                var adress = placesInPlan[i].Adress.Split("\n\n\n")[(int)user.Language];

                result += $"{i + 1}) {name}\n{adress}\n\n";
            }

            return result;
        }
    }
}
