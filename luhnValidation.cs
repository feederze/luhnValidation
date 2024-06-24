using Xunit;

namespace luhnValidation
{
    public class luhnValidation
    {

        [Theory]
        [InlineData("4111 1111 1111 1111", true)]
        [InlineData("4ab1asd1as1 1asd111 11gfd11 1111", true)]
        [InlineData("123456489", false)]
        [InlineData("17893729974", true)]
        [InlineData("5610591081018250", false)]
        [InlineData("abcdefghijk", false)]
        public void TestIsValidLuhn(string cardNumber, bool expected)
        {
            bool result = isValidLuhn(cardNumber);
            Assert.Equal(expected, result);
        }
        static public bool isValidLuhn(string? cardNumber)
        {
            // do not accept empty input
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return false;
            }

            //trim spaces on both ends
            cardNumber = cardNumber.Trim();

            //luhn method
            //code based on the examples on wikipedia.
            int sum = 0;
            bool isDoublingValue = true;
            int checkDigit = 0;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                //return false if non-digit character is found
                if (!char.IsDigit(cardNumber[i]))
                {
                    continue;
                }
                int n = int.Parse(cardNumber[i].ToString());


                //the last digit is used for validation.
                if (i == cardNumber.Length - 1)
                {
                    checkDigit = n;
                    continue;
                }

                if (isDoublingValue)
                {
                    n *= 2;

                    //if greater than 10 use the sum of two digits
                    //since value range is 0~18
                    //simplify this by -9
                    if (n > 9)
                    {
                        n -= 9;
                    }
                }

                sum += n;
                isDoublingValue = !isDoublingValue;
            }
            //compare given check digit and the check digit by calculation
            //pass if results match
            return (checkDigit == (10 - (sum % 10)));
        }


    }
}
