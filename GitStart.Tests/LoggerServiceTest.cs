using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using System;
using GitStart.Services;
using Xunit;

namespace GitStart.Tests
{
    public class LoggerServiceTests
    {
        public LoggerServiceTests()
        {
            // Настройка для использования TestCorrelator
            TestCorrelator.CreateContext(); // Обнуляем собранные логи перед каждым тестом
        }

        [Fact]
        public void LogInfo_ShouldLogInfoMessage()
        {
            // Arrange
            string expectedMessage = "Test Info Log";

            // Обновляем логгер для тестов
            LoggerService.Logger = new LoggerConfiguration()
                .WriteTo.TestCorrelator() // Записываем логи в TestCorrelator
                .CreateLogger();

            // Act
            LoggerService.LogInfo(expectedMessage);

            // Assert
            var logEvents = TestCorrelator.GetLogEventsFromCurrentContext(); // Получаем логи текущего потока
            Assert.Single(logEvents); // Проверяем, что одно сообщение записано
            Assert.Contains(expectedMessage, logEvents[0].MessageTemplate.Text); // Проверяем, что сообщение присутствует
            Assert.Equal(LogEventLevel.Information, logEvents[0].Level); // Проверяем уровень логирования
        }

        [Fact]
        public void LogError_ShouldLogErrorMessage()
        {
            // Arrange
            string expectedMessage = "Test Error Log";
            var exception = new Exception("Test Exception");

            // Обновляем логгер для тестов
            LoggerService.Logger = new LoggerConfiguration()
                .WriteTo.TestCorrelator() // Записываем логи в TestCorrelator
                .CreateLogger();

            // Act
            LoggerService.LogError(exception, expectedMessage);

            // Assert
            var logEvents = TestCorrelator.GetLogEventsFromCurrentContext(); // Получаем логи текущего потока
            Assert.Single(logEvents); // Проверяем, что одно сообщение записано
            Assert.Contains(expectedMessage, logEvents[0].MessageTemplate.Text); // Проверяем, что сообщение присутствует
            Assert.Equal(LogEventLevel.Error, logEvents[0].Level); // Проверяем уровень логирования
            Assert.Equal(exception.Message, logEvents[0].Exception?.Message); // Проверяем, что исключение записано
        }

        [Fact]
        public void LogWarning_ShouldLogWarningMessage()
        {
            // Arrange
            string expectedMessage = "Test Warning Log";

            // Обновляем логгер для тестов
            LoggerService.Logger = new LoggerConfiguration()
                .WriteTo.TestCorrelator() // Записываем логи в TestCorrelator
                .CreateLogger();

            // Act
            LoggerService.LogWarning(expectedMessage);

            // Assert
            var logEvents = TestCorrelator.GetLogEventsFromCurrentContext(); // Получаем логи текущего потока
            Assert.Single(logEvents); // Проверяем, что одно сообщение записано
            Assert.Contains(expectedMessage, logEvents[0].MessageTemplate.Text); // Проверяем, что сообщение присутствует
            Assert.Equal(LogEventLevel.Warning, logEvents[0].Level); // Проверяем уровень логирования
        }

        [Fact]
        public void LogInfo_ShouldLogMultipleMessages()
        {
            // Arrange
            string message1 = "First Info Message";
            string message2 = "Second Info Message";

            // Обновляем логгер для тестов
            LoggerService.Logger = new LoggerConfiguration()
                .WriteTo.TestCorrelator() // Записываем логи в TestCorrelator
                .CreateLogger();

            // Act
            LoggerService.LogInfo(message1);
            LoggerService.LogInfo(message2);

            // Assert
            var logEvents = TestCorrelator.GetLogEventsFromCurrentContext(); // Получаем логи текущего потока
            Assert.Equal(2, logEvents.Count); // Проверяем, что два сообщения записаны
            Assert.Contains(message1, logEvents[0].MessageTemplate.Text); // Проверяем, что первое сообщение присутствует
            Assert.Contains(message2, logEvents[1].MessageTemplate.Text); // Проверяем, что второе сообщение присутствует
        }
    }
}
