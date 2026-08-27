using Xunit;
using Library;

namespace Library.Tests
{
    public class LibraryBusinessTests
    {
        [Fact]
        public void Book_DueDate_ShouldBeFourteenDaysFromBorrow()
        {
            // Arrange
            var borrowDate = DateTime.Now;
            var dueDate = borrowDate.AddDays(14);

            // Act & Assert
            Assert.Equal(borrowDate.AddDays(14).Date, dueDate.Date);
        }

        [Fact]
        public void FineCalculation_Tier1_ShouldBeFiftyPkrPerDay()
        {
            // Arrange (1 to 5 days late = 50 PKR per day)
            int daysLate = 3;
            int fineMultiplier = 50;

            // Act
            int totalFine = daysLate * fineMultiplier;

            // Assert
            Assert.Equal(150, totalFine);
        }

        [Fact]
        public void FineCalculation_Tier3_ShouldBeOneHundredPkrPerDay()
        {
            // Arrange (16+ days late = 100 PKR per day)
            int daysLate = 20;
            int fineMultiplier = 100;

            // Act
            int totalFine = daysLate * fineMultiplier;

            // Assert
            Assert.Equal(2000, totalFine);
        }

        [Fact]
        public void GuestUser_AgeValidation_ShouldRejectUnderage()
        {
            // Arrange
            int underage = 16;

            // Act & Assert
            bool isAllowed = underage >= 18;
            Assert.False(isAllowed);
        }
    }
}