# xUnit Customization #
Решение иллюстрирует возможности кастомизации тестового фреймворка
xUnit на примере управления фича-флагами (feature-toggles) при
автоматизации тестирования API веб-приложений.

## Структура решения ##

- <span style="color: green;">WeatherForecast.Api</span> - тестируемое веб-приложение.
- <span style="color: green;">WeatherForecast.Api.Clients</span> - клиенты к тестируемому веб-приложению.
- <span style="color: green;">WeatherForecast.Api.Contracts</span> - контракты к клиентам.
- <span style="color: green;">WeatherForecast.Api.TestFramework</span> - основной проект с кастомизацией тестового фреймворка.
- <span style="color: green;">WeatherForecast.Api.Tests</span> - проект с тестами для демонстрации.

## Запуск ##
1. Сначала запускаем веб-приложение - `WeatherForecast.Api`.
2. Запускаем тесты. Если работаете в Visual Studio, то пригодятся Test Explorer
и вкладка Output с выбранным источником вывода "Tests".
