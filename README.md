# MiodenusAnimationConverter

*MiodenusAnimationConverter* - это консольное приложение, входящее в состав *Miodenus Project*, отвечающее за преобразование входных данных ([см. ниже](#входные-данные)) в итоговый видеофайл, содержащий анимацию сборки.

## Демо

Ниже представлена демонстрационная сборка редуктора привода лебёдки, созданная в *MiodenusAnimationConverter*.

<p align="center"><img src="https://user-images.githubusercontent.com/57591626/162587743-1500fce5-f069-4e37-bf47-8021115e6a65.gif" alt="Демо" /></p>

## Входные данные

На вход данное приложение принимает файл, описывающий анимацию сборки. Его структура представлена ниже.

### Анимационный файл

Ниже представлен пример содержимого анимационного файла.

```json
{
  "animationInfo":
  {
    "type": "maf",
    "version": "1.0",
    "name": "AnimationDemo",
    "videoType": "mp4",
    "videoName": "ResultVideo",
    "timeLength": 3600,
    "fps": 60,
    "frameWidth": 600,
    "frameHeight": 600
  },
  "modelsInfo":
  [
    {
      "name": "bolt_25x8",
      "type": "stl",
      "filename": "path/to/bolt_25x8.stl",
      "color": [0.0, 1.0, 0.0],
      "baseTransformation":
      {
        "location": [0.0, 0.0, 0.0],
        "rotation":
        {
          "angle": 5.0,
          "unit": "rad",
          "vector": [0.0, 1.0, 0.0]
        },
        "scale": [0.5, 0.5, 0.5]
      }
    },
    {
      "name": "nut_6x9",
      "type": "stl",
      "filename": "path/to/nut_6x9.stl",
      "color": [1.0, 0.0, 0.0],
      "baseTransformation":
      {
        "location": [0.0, 0.0, 20.0],
        "rotation":
        {
          "angle": 90.0,
          "unit": "deg",
          "vector": [1.0, 0.0, 0.0]
        },
        "scale": [0.5, 0.5, 0.5]
      }
    }
  ],
  "actions":
  [
    {
      "name": "nut rotation",
      "states":
      [
        {
          "time": 0,
          "isModelVisible": true,
          "color": [0.0, 1.0, 0.0],
          "transformation":
          {
            "location": [0.0, 0.0, 0.0],
            "rotation":
            {
              "angle": 10.0,
              "unit": "deg",
              "vector": [1.0, 0.0, 0.0]
            },
            "scale": [1.0, 1.0, 1.0]
          }
        },
        {
          "time": 1500,
          "transformation":
          {
            "location": [0.0, 0.0, -20.0],
            "rotation":
            {
              "angle": 180.0,
              "unit": "deg",
              "vector": [1.0, 0.0, 0.0]
            }
          }
        },
        {
          "time": 3000,
          "transformation":
          {
            "rotation":
            {
              "angle": -7.0,
              "vector": [0.0, 1.0, 0.0]
            }
          }
        }
      ]
    }
  ],
  "bindings":
  [
    {
      "modelName": "nut_6x9",
      "actionName": "nut rotation",
      "startTime": 10,
      "timeLength": 3500,
      "useInterpolation": true
    },
    {
      "modelName": "bolt_25x8",
      "actionName": "nut rotation",
      "startTime": 2000,
      "timeLength": 100,
      "useInterpolation": false
    }
  ]
}
```

## Выходные данные

В результате работы приложения будет получен видеофайл, содержащий анимацию сборки. В случае передачи особых аргументов результат работы приложения может быть другим ([см. ниже](#таблица-аргументов)).

## Запуск из консоли
### Таблица аргументов

| Обязательный аргумент | Ключ | Значение | Примечание |
| :---: | :---: | :---: | --- |
| Да | `-a` `--animation` | Путь до файла, описывающего анимацию сборки | Путь лучше заключать в двойные `"` кавычки. |
| Нет | `-q` `--quiet` | Отсутствует | В данном режиме приложение не будет выводить сообщения (предупреждения, ошибки...) в консоль. Вывод информации в журнал не будет остановлен. |
| Нет | `-v` `--view` | Номер кадра, который будет загружен для просмотра `*` | Будет запущено графическое окно (размер и другие параметры берутся из файла, описывающего анимацию сборки) в котором будет преставлено состояние анимации (сцена, положение камер...) на момент указанного кадра. При помощи определенных комбинаций клавиш (см. ниже) пользователь сможет осмотреть сцену. Если значение аргумента равно `0` — будет показана вся анимация. В данном режиме приложение не будет производить запись анимации. Диапазон допустимых значений: от `0` до `числа кадров в итоговом видео сборки`. |
| Нет | `-f` `--frame` | Номер кадра, который требуется получить в виде файла изображения `*` | В папке `screenshots` появится файл изображения указанного кадра. Наименование файла будет иметь следующий вид: `<номер-кадра>_<дата>.png`, где `<дата>` имеет формат `ГГГГ_ММ_ДД_чч_мм_сс`. В случае если файл с таким именем уже существует — он будет перезаписан. В данном режиме приложение не будет производить запись анимации. Диапазон допустимых значений: от `1` до `числа кадров в итоговом видео сборки`. |
| Нет | `--help` | Отсутствует | В консоль будет выведена подробная справка о работе программы, после чего приложение завершит свою работу. |
| Нет | `--version` | Отсутствует | В консоль будет выведена информация о версии установленной программы, после чего приложение завершит свою работу. |

`*` — *Аргументы `--view` и `--frame` не могут применяться одновременно!*

### Пример запуска из консоли

Команда, представленная ниже, создаст итоговый видеофайл сборки, используя анимационный файл *animation.maf*. Приложение не будет выводить какие-либо сообщения в консоль.

`$ ./MiodenusAnimationConverter --animation "path/to/animation.maf" --quiet`

## Установка

Можно воспользоваться уже [скомпилированными исходниками](https://cloud.mail.ru/public/WjBw/uoZge8X91) (Windows 10 / Kubuntu 21.10).

### Особенности установки на Windows

В директорию проекта (где находятся файлы с расширением .dll) необходимо поместить файл *ffmpeg.exe*.

### Особенности установки на Linux

Приложение было протестировано под ОС [Kubuntu 21.10](https://kubuntu.org/).

Рекомендуется использовать проприетарные драйвера для видеокарты.

Установка [.NET 5](https://docs.microsoft.com/ru-ru/dotnet/core/install/linux-ubuntu) в Kubuntu.

Установка зависимостей:

`sudo apt install -y libgdiplus`

`sudo apt install ffmpeg`
