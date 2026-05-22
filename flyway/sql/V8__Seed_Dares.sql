-- 54 unique dares × 2 languages = 108 rows
-- Categories: physical, social, mental, creative, wellness, discipline

-- ═══════════════════════════════════════════════════════════════════
--  PHYSICAL → Strength
-- ═══════════════════════════════════════════════════════════════════

INSERT INTO public.random_dare (uuid, language, context, experience_gained, given_time, category) VALUES
  (uuid_generate_v4(), 'en'::language, 'Do 50 push-ups in one session',        25, 20,  'physical'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Зробіть 50 віджимань за один підхід',  25, 20,  'physical'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Run 5 km without stopping',             35, 60,  'physical'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Пробіжіть 5 км без зупинок',           35, 60,  'physical'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Hold a plank for 2 minutes straight',   20, 10,  'physical'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Тримайте планку 2 хвилини поспіль',    20, 10,  'physical'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Complete 100 squats',                   25, 20,  'physical'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Виконайте 100 присідань',              25, 20,  'physical'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Walk 10,000 steps in one day',          30, 0,   'physical'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Пройдіть 10 000 кроків за один день',  30, 0,   'physical'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Do 20 pull-ups',                        30, 20,  'physical'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Зробіть 20 підтягувань',               30, 20,  'physical'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Swim 500 metres',                       35, 30,  'physical'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Пропливіть 500 метрів',                35, 30,  'physical'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Cycle 15 km',                           40, 60,  'physical'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Проїдьте 15 км на велосипеді',         40, 60,  'physical'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Try a new sport for the first time',    40, 90,  'physical'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Спробуйте новий вид спорту вперше',    40, 90,  'physical'::dare_category);

-- ═══════════════════════════════════════════════════════════════════
--  SOCIAL → Charisma
-- ═══════════════════════════════════════════════════════════════════

INSERT INTO public.random_dare (uuid, language, context, experience_gained, given_time, category) VALUES
  (uuid_generate_v4(), 'en'::language, 'Pay a genuine compliment to 5 different people today',         15, 0,   'social'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Зробіть щирий комплімент 5 різним людям сьогодні',            15, 0,   'social'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Call a friend you haven''t spoken to in months',               25, 30,  'social'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Зателефонуйте другу, з яким давно не спілкувалися',           25, 30,  'social'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Attend a social event alone and meet 3 new people',            45, 180, 'social'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Відвідайте соціальний захід наодинці та познайомтеся з 3 новими людьми', 45, 180, 'social'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Volunteer at a local charity for a day',                       60, 0,   'social'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Поволонтерьте у місцевій благодійній організації один день',  60, 0,   'social'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Teach someone a skill you know well',                          35, 60,  'social'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Навчіть когось навичці, якою ви добре володієте',             35, 60,  'social'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Write a thank-you note to someone who helped you',             20, 30,  'social'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Напишіть подяку людині, яка вам допомогла',                  20, 30,  'social'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Organise a team lunch or group outing',                        40, 60,  'social'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Організуйте спільний обід або вихід із командою',             40, 60,  'social'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Reconnect with an old colleague or classmate',                 25, 30,  'social'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Відновіть зв''язок із колишнім колегою чи однокласником',    25, 30,  'social'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Leave a genuine positive review for a local business',         10, 10,  'social'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Залиште щирий позитивний відгук про місцевий бізнес',        10, 10,  'social'::dare_category);

-- ═══════════════════════════════════════════════════════════════════
--  MENTAL → Intelligence
-- ═══════════════════════════════════════════════════════════════════

INSERT INTO public.random_dare (uuid, language, context, experience_gained, given_time, category) VALUES
  (uuid_generate_v4(), 'en'::language, 'Read 30 pages of a non-fiction book',                          20, 60,  'mental'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Прочитайте 30 сторінок науково-популярної книги',             20, 60,  'mental'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Learn 10 new words in a foreign language',                     20, 30,  'mental'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Вивчіть 10 нових слів іноземною мовою',                      20, 30,  'mental'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Watch a documentary and write a short summary',                25, 90,  'mental'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Подивіться документальний фільм і напишіть короткий конспект', 25, 90, 'mental'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Spend a full day without social media',                        35, 0,   'mental'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Проведіть повний день без соціальних мереж',                  35, 0,   'mental'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Meditate for 20 minutes focusing only on your breath',         20, 20,  'mental'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Медитуйте 20 хвилин, зосередившись лише на диханні',         20, 20,  'mental'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Write down 10 ideas for a project or business',                25, 30,  'mental'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Запишіть 10 ідей для проєкту або бізнесу',                   25, 30,  'mental'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Solve a Sudoku on hard difficulty',                            20, 30,  'mental'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Розв''яжіть судоку на складному рівні',                      20, 30,  'mental'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Listen to a podcast on a topic completely new to you',         20, 60,  'mental'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Прослухайте подкаст на абсолютно нову для вас тему',          20, 60,  'mental'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Plan your entire week in detail on a Sunday evening',          25, 30,  'mental'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Сплануйте весь тиждень детально у неділю ввечері',            25, 30,  'mental'::dare_category);

-- ═══════════════════════════════════════════════════════════════════
--  CREATIVE → Dexterity
-- ═══════════════════════════════════════════════════════════════════

INSERT INTO public.random_dare (uuid, language, context, experience_gained, given_time, category) VALUES
  (uuid_generate_v4(), 'en'::language, 'Draw a portrait from life',                                    25, 60,  'creative'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Намалюйте портрет з натури',                                  25, 60,  'creative'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Write a short poem',                                           15, 20,  'creative'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Напишіть короткий вірш',                                      15, 20,  'creative'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Cook a dish from a cuisine you''ve never tried before',        30, 90,  'creative'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Приготуйте страву з кухні, яку ви ніколи не пробували',      30, 90,  'creative'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Photograph 10 things that make you happy today',               20, 60,  'creative'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Сфотографуйте 10 речей, які роблять вас щасливими',           20, 60,  'creative'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Spend 30 minutes learning the basics of any instrument',       25, 30,  'creative'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Витратьте 30 хвилин на вивчення основ будь-якого інструменту', 25, 30, 'creative'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Write the opening page of a short story',                      20, 30,  'creative'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Напишіть першу сторінку короткого оповідання',               20, 30,  'creative'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Bake bread from scratch',                                      35, 180, 'creative'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Спечіть хліб з нуля',                                        35, 180, 'creative'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Invent a new recipe using only what''s in your fridge',        25, 45,  'creative'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Придумайте новий рецепт із того, що є у холодильнику',        25, 45,  'creative'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Write a handwritten letter and send it to someone',            20, 30,  'creative'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Напишіть лист від руки і надішліть його комусь',              20, 30,  'creative'::dare_category);

-- ═══════════════════════════════════════════════════════════════════
--  WELLNESS → Vitality
-- ═══════════════════════════════════════════════════════════════════

INSERT INTO public.random_dare (uuid, language, context, experience_gained, given_time, category) VALUES
  (uuid_generate_v4(), 'en'::language, 'Drink 2 litres of water throughout the day',                   15, 0,   'wellness'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Випийте 2 літри води протягом дня',                           15, 0,   'wellness'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Take a cold shower',                                           20, 5,   'wellness'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Прийміть холодний душ',                                       20, 5,   'wellness'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Spend 30 minutes in nature without your phone',                20, 30,  'wellness'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Проведіть 30 хвилин на природі без телефону',                 20, 30,  'wellness'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Cook all your meals at home for one day',                      25, 0,   'wellness'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Готуйте всі страви вдома один день',                          25, 0,   'wellness'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Sleep 8 hours and track it',                                   20, 0,   'wellness'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Поспіть 8 годин і відстежте це',                              20, 0,   'wellness'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Prep healthy meals for the next 3 days',                       35, 90,  'wellness'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Підготуйте здорову їжу на наступні 3 дні',                   35, 90,  'wellness'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Replace all sugary drinks with water for one day',             20, 0,   'wellness'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Замініть усі солодкі напої на воду на один день',             20, 0,   'wellness'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Take a 20-minute walk in a park or green space',               15, 20,  'wellness'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Прогуляйтеся 20 хвилин у парку або зеленій зоні',            15, 20,  'wellness'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Spend 10 minutes in morning sunlight before checking your phone', 10, 10, 'wellness'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Проведіть 10 хвилин на ранковому сонці перед тим, як перевіряти телефон', 10, 10, 'wellness'::dare_category);

-- ═══════════════════════════════════════════════════════════════════
--  DISCIPLINE → Willpower
-- ═══════════════════════════════════════════════════════════════════

INSERT INTO public.random_dare (uuid, language, context, experience_gained, given_time, category) VALUES
  (uuid_generate_v4(), 'en'::language, 'Wake up at 6 AM without hitting snooze',                       25, 0,   'discipline'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Прокиньтеся о 6 ранку без вимкнення будильника',              25, 0,   'discipline'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Do not complain about anything for 24 hours',                  30, 0,   'discipline'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Не скаржіться ні на що протягом 24 годин',                    30, 0,   'discipline'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Complete your full to-do list without procrastinating',         25, 0,   'discipline'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Виконайте весь список справ без прокрастинації',               25, 0,   'discipline'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Give up one bad habit for 3 days in a row',                    35, 0,   'discipline'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Відмовтеся від однієї поганої звички на 3 дні поспіль',       35, 0,   'discipline'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Don''t check your phone for the first hour after waking',      20, 0,   'discipline'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Не перевіряйте телефон першу годину після пробудження',       20, 0,   'discipline'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Follow your planned schedule for an entire day',                30, 0,   'discipline'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Дотримуйтеся запланованого розкладу весь день',               30, 0,   'discipline'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Spend no money on non-essentials for 3 days',                  30, 0,   'discipline'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Не витрачайте гроші на несуттєві речі 3 дні',                 30, 0,   'discipline'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Write your goals for the next 3 months',                       25, 30,  'discipline'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Запишіть свої цілі на наступні 3 місяці',                    25, 30,  'discipline'::dare_category),

  (uuid_generate_v4(), 'en'::language, 'Do one thing you''ve been avoiding for over a week',           35, 60,  'discipline'::dare_category),
  (uuid_generate_v4(), 'ua'::language, 'Зробіть те, що відкладали вже більше тижня',                  35, 60,  'discipline'::dare_category);
