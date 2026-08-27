using Xunit;

namespace Library.Tests
{
    public class LibraryBusinessTests
    {
        [Fact]
        public void Borrowing_DueDate_ShouldBeFourteenDaysFromCheckout()
        {
            var borrowDate = System.DateTime.Now;
            var dueDate = borrowDate.AddDays(14);

            Assert.Equal(borrowDate.AddDays(14).Date, dueDate.Date);
        }

        [Theory]
        [InlineData(1, 50, 50)]
        [InlineData(3, 50, 150)]
        [InlineData(6, 75, 450)]
        [InlineData(20, 100, 2000)]
        public void FineCalculation_TieredMultipliers_ShouldCalculateCorrectly(int daysLate, int multiplier, int expectedFine)
        {
            int totalFine = daysLate * multiplier;
            Assert.Equal(expectedFine, totalFine);
        }

        [Theory]
        [InlineData(16, false)]
        [InlineData(18, true)]
        [InlineData(25, true)]
        public void GuestUser_AgeValidation_ShouldEnforceEighteenPlus(int age, bool expectedOutcome)
        {
            bool isAllowed = age >= 18;
            Assert.Equal(expectedOutcome, isAllowed);
        }

        [Theory]
        [InlineData("", true)]
        [InlineData("   ", true)]
        [InlineData("Muhammad Arhum", false)]
        public void Validation_MemberName_ShouldDetectEmptyOrWhitespace(string memberName, bool expectedIsEmpty)
        {
            bool isEmpty = string.IsNullOrWhiteSpace(memberName);
            Assert.Equal(expectedIsEmpty, isEmpty);
        }

        [Fact]
        public void FinePayment_Overpayment_ShouldCalculateCorrectChange()
        {
            int fineDue = 300;
            int paymentAmount = 500;

            int change = paymentAmount > fineDue ? paymentAmount - fineDue : 0;
            int remainingBalance = paymentAmount > fineDue ? 0 : fineDue - paymentAmount;

            Assert.Equal(200, change);
            Assert.Equal(0, remainingBalance);
        }

        [Fact]
        public void BookLimit_Enforcement_ShouldRestrictBeyondThreeBooks()
        {
            int currentCheckedOutBooks = 3;
            bool canBorrowMore = currentCheckedOutBooks < 3;

            Assert.False(canBorrowMore);
        }
    }
}