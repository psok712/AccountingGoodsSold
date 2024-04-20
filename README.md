# Ozon Route 256 — Kafka Homework

## Домашнее задание

Наш маленький стартап Кислород.Ру перерос в серьезный бизнес, и нам нужно научиться в учёт проданного товара, а также (
опционально) в аналитику продаваемого товара

В проде у нас есть сервис заказов, который при создании заказа отправляет в топик Кафки событие об этом (в рамках ДЗ его
роль выполняет консольное приложение, которое нещадно генерирует события в этот топик).

Вот формат этого события:

```json
{
    "moment": "timestamp",
    "order_id": "long",
    "user_id": "long",
    "warehouse_id": "long",
    "positions": {
        "item_id": "long",
        "quantity": "int",
        "price": {
            "currency": "RUR|KZT",
            "units": "long",
            "nanos": "int"
        }
    },
    "status": "Created|Canceled|Delivered"
}
```

Пояснения к контракту:

* Первое событие по заказу всегда будет created
* Второе событие — всегда canceled или delivered. Но второго может и не быть
* Объект price повторяет тип google.type.money.
*
    * currency - валюта,
*
    * units - целая часть
*
    * nanos - дробная часть в диапазоне 0..1, умноженная на 1,000,000,000.
*
    * Пример: цена в 5316 рубля 78 копеек будет отражена как currency="RUB", units=5316, nanos = 780_000_000

### Домашнее задание — реализовать сервис

Написать сервис, который будет читать этот топик и считать витрину данных, которую будет сохранять в базу данных в виде
таблицы.
Реализовать учет товаров — по каждому item_id: сколько зарезервировано (created), сколько продано (переход created →
delivered), сколько отменено (created → cancelled). Также стоит хранить дату и время последнего обновления данной
информации.
За основную часть задания максмум 8 баллов.

Задания на дополнительные баллы (+1 балл за каждый пункт):

* Учёт денежных средств к уплате каждому продавцу. item_id всегд состоит из 12 цифр. Первые 6 цифр у item_id —
  идентификатор продавца данного товара, следующие 6 цифр — идентификатор товара у данного продавца(то
  есть `213176086538` и `213176768102` — товары одного продавца, а `213176086538` и `133676086538` — разные товары
  разных продавцов, несмотря на одинаковость последних 6 цифр). Идентификатор продавца не может начинаться с 0 (
  минимальный идентификатор продавца `100000`). Необходимо считать продажи (delivered) каждого товара в разной валюте и
  их количество.
* Отсутствие ошибок подсчета при наличии 2+ партиций и 2+ инстансов сервиса. Что за ошибка может возникнуть следует
  догадаться самостоятельно.

Максимум за это задание можно заработать 10 баллов.

### Сроки

Вы получили данное задание 13 апреля. У вас есть неделя (до 20 апреля), что бы сообщить тьютору о выполнении задания.

## Docker cheat sheet

* Если у вас всё ещё нет docker — его нужно поставить.
* Запустить docker контейнеры (БД): `docker compose up -d`
* Остановить docker контейнеры (БД): `docker compose down`
* Остановить и почистить docker от данных: `docker compose down -v`
* Docker поломался: `docker system prune`

## Kafka cheat sheet

* Заводите Kafka через docker. Пример docker-compose-файла с Kafka находится прямо в этом репозитории.
* Откройте ваш hosts-файл (для windows это `c:\windows\system32\drivers\etc\hosts`) Добавьте туда
  строчку `127.0.0.1 kafka`.
* Offset Explorer (ранее называлось Kafka Tool): https://www.kafkatool.com/ — позволяет читать и писать в Apache Kafka
  через простой UI. Да, писать и читать protobuf почти нереально. Но это отдельная боль в нашем мире.
* Как настроить Offset Explorer?
*
    * Clusters → Add New Connection
*
    * Cluster Name → Любое имя
*
    * Вкладка Advanced → Bootstrap Servers → написать `kafka:9092`
*
    * Test, Add
* Серьезные девчата и пацаны качают Kafka (https://kafka.apache.org/downloads) и используют sh/bat-файлы оттуда, чтобы
  работать с локальной/докер/стейдж/прод Kafka. Но мы тут все несерьезные, и используем такое только в случае крайней
  необходимости.

### FAQ

Q: Я нашёл баг в генераторе!

A: Преподаватель — такой же человек, как и вы. Не забудьте сообщить об этом баге в чат

Q: Как организовать чтение в сервисе?

A:
Используйте [Background Hosted Service](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio)