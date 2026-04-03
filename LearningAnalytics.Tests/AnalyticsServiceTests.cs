using System;
using System.Collections.Generic;
using System.Linq;
using LearningAnalytics.Core.Models;
using LearningAnalytics.Services.Services;
using Xunit;

namespace LearningAnalytics.Tests
{
    /// <summary>
    /// Tests unitarios para AnalyticsService
    /// </summary>
    public class AnalyticsServiceTests
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsServiceTests()
        {
            _analyticsService = new AnalyticsService();
        }

        [Fact]
        public void AnalyzeStudents_EmptyList_ReturnsEmptyResult()
        {
            // Arrange
            var students = new List<Student>();

            // Act
            var result = _analyticsService.AnalyzeStudents(students);

            // Assert
            Assert.Equal(0, result.TotalStudents);
            Assert.Equal(0, result.AverageGrade);
            Assert.Equal(0, result.AverageParticipation);
            Assert.Equal(0, result.PassingStudents);
            Assert.Equal(0, result.FailingStudents);
        }

        [Fact]
        public void AnalyzeStudents_ValidList_ReturnsCorrectResults()
        {
            // Arrange
            var students = CreateTestStudents();

            // Act
            var result = _analyticsService.AnalyzeStudents(students);

            // Assert
            Assert.Equal(5, result.TotalStudents);
            Assert.Equal(7.3, result.AverageGrade, 1);
            Assert.Equal(6.4, result.AverageParticipation, 1);
            Assert.Equal(3, result.PassingStudents);
            Assert.Equal(2, result.FailingStudents);
        }

        [Fact]
        public void CalculateAverageGrade_ValidList_ReturnsCorrectAverage()
        {
            // Arrange
            var students = CreateTestStudents();

            // Act
            var average = _analyticsService.CalculateAverageGrade(students);

            // Assert
            Assert.Equal(7.3, average, 1);
        }

        [Fact]
        public void CalculateAverageGrade_EmptyList_ReturnsZero()
        {
            // Arrange
            var students = new List<Student>();

            // Act
            var average = _analyticsService.CalculateAverageGrade(students);

            // Assert
            Assert.Equal(0, average);
        }

        [Fact]
        public void CalculateAverageParticipation_ValidList_ReturnsCorrectAverage()
        {
            // Arrange
            var students = CreateTestStudents();

            // Act
            var average = _analyticsService.CalculateAverageParticipation(students);

            // Assert
            Assert.Equal(6.4, average, 1);
        }

        [Fact]
        public void IdentifyAtRiskStudents_ReturnsCorrectStudents()
        {
            // Arrange
            var students = CreateTestStudents();

            // Act
            var atRiskStudents = _analyticsService.IdentifyAtRiskStudents(students);

            // Assert
            Assert.Equal(3, atRiskStudents.Count); // Students with grade < 5 or participation < 3
            Assert.Contains(atRiskStudents, s => s.Name == "Carlos López"); // Grade 4.5
            Assert.Contains(atRiskStudents, s => s.Name == "Ana Martínez"); // Participation 2.5
        }

        [Fact]
        public void CalculateParticipationGradeCorrelation_PositiveCorrelation_ReturnsPositiveValue()
        {
            // Arrange
            var students = CreateTestStudents();

            // Act
            var correlation = _analyticsService.CalculateParticipationGradeCorrelation(students);

            // Assert
            Assert.True(correlation > 0); // Should be positive correlation
        }

        [Fact]
        public void CalculateParticipationGradeCorrelation_EmptyList_ReturnsZero()
        {
            // Arrange
            var students = new List<Student>();

            // Act
            var correlation = _analyticsService.CalculateParticipationGradeCorrelation(students);

            // Assert
            Assert.Equal(0, correlation);
        }

        [Fact]
        public void CalculateParticipationGradeCorrelation_SingleStudent_ReturnsZero()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Name = "Test", Grade = 8.0, Participation = 7.0 }
            };

            // Act
            var correlation = _analyticsService.CalculateParticipationGradeCorrelation(students);

            // Assert
            Assert.Equal(0, correlation);
        }

        [Fact]
        public void GenerateRecommendations_ValidList_ReturnsRecommendations()
        {
            // Arrange
            var students = CreateTestStudents();

            // Act
            var recommendations = _analyticsService.GenerateRecommendations(students);

            // Assert
            Assert.NotEmpty(recommendations);
            Assert.True(recommendations.Any(r => r.Contains("riesgo"))); // Should mention at-risk students
        }

        [Fact]
        public void GetDescriptiveStatistics_ValidList_ReturnsCorrectStatistics()
        {
            // Arrange
            var students = CreateTestStudents();

            // Act
            var stats = _analyticsService.GetDescriptiveStatistics(students);

            // Assert
            Assert.Equal(7.3, stats.GradeMean, 1);
            Assert.Equal(6.4, stats.ParticipationMean, 1);
            Assert.True(stats.GradeMin <= stats.GradeMax);
            Assert.True(stats.ParticipationMin <= stats.ParticipationMax);
        }

        [Fact]
        public void SegmentStudents_ValidList_ReturnsCorrectSegments()
        {
            // Arrange
            var students = CreateTestStudents();

            // Act
            var segmentation = _analyticsService.SegmentStudents(students);

            // Assert
            Assert.Equal(1, segmentation.Excellent.Count); // María García (9.2)
            Assert.Equal(1, segmentation.Good.Count);      // Juan Pérez (8.5)
            Assert.Equal(1, segmentation.Average.Count);    // Luis Rodríguez (6.8)
            Assert.Equal(1, segmentation.Poor.Count);       // Carlos López (4.5)
            Assert.Equal(1, segmentation.Critical.Count);   // Ana Martínez (3.2)
        }

        [Fact]
        public void AnalyzeStudents_PerfectScores_ReturnsCorrectResults()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Name = "Perfect 1", Grade = 10.0, Participation = 10.0 },
                new Student { Name = "Perfect 2", Grade = 9.8, Participation = 9.5 }
            };

            // Act
            var result = _analyticsService.AnalyzeStudents(students);

            // Assert
            Assert.Equal(2, result.TotalStudents);
            Assert.Equal(2, result.PassingStudents);
            Assert.Equal(0, result.FailingStudents);
            Assert.Equal(0, result.AtRiskStudents);
            Assert.True(result.AverageGrade > 9.0);
        }

        [Fact]
        public void AnalyzeStudents_AllFailing_ReturnsCorrectResults()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Name = "Failing 1", Grade = 2.0, Participation = 1.0 },
                new Student { Name = "Failing 2", Grade = 3.5, Participation = 2.0 }
            };

            // Act
            var result = _analyticsService.AnalyzeStudents(students);

            // Assert
            Assert.Equal(2, result.TotalStudents);
            Assert.Equal(0, result.PassingStudents);
            Assert.Equal(2, result.FailingStudents);
            Assert.Equal(2, result.AtRiskStudents);
            Assert.True(result.AverageGrade < 5.0);
        }

        [Fact]
        public void Student_IsPassing_ReturnsCorrectValue()
        {
            // Arrange
            var passingStudent = new Student { Grade = 6.0 };
            var failingStudent = new Student { Grade = 4.9 };

            // Act & Assert
            Assert.True(passingStudent.IsPassing);
            Assert.False(failingStudent.IsPassing);
        }

        [Fact]
        public void Student_IsAtRisk_ReturnsCorrectValue()
        {
            // Arrange
            var riskByGrade = new Student { Grade = 4.0, Participation = 8.0 };
            var riskByParticipation = new Student { Grade = 8.0, Participation = 2.0 };
            var safeStudent = new Student { Grade = 7.0, Participation = 7.0 };

            // Act & Assert
            Assert.True(riskByGrade.IsAtRisk);
            Assert.True(riskByParticipation.IsAtRisk);
            Assert.False(safeStudent.IsAtRisk);
        }

        private List<Student> CreateTestStudents()
        {
            return new List<Student>
            {
                new Student { Name = "Juan Pérez", Grade = 8.5, Participation = 7.5 },
                new Student { Name = "María García", Grade = 9.2, Participation = 8.8 },
                new Student { Name = "Carlos López", Grade = 4.5, Participation = 6.0 },
                new Student { Name = "Ana Martínez", Grade = 6.8, Participation = 2.5 },
                new Student { Name = "Luis Rodríguez", Grade = 7.5, Participation = 7.2 }
            };
        }
    }
}
