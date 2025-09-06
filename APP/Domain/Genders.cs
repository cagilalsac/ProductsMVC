namespace APP.Domain
{
    // Enums are used for predefined values and help not to remember or check each element's value when being used.
    // Getting the value of an enum element: int womanValue = (int)Genders.Woman;
    // Getting the text of an enum element: string manText = Genders.Man.ToString();
    // Way 1:
    //public enum Genders
    //{
    //    Woman, // if no assignment, will start from 0
    //    Man // will automatically get the next value 1 and other elements will continue to get the next values after 1
    //}
    // Way 2:
    //public enum Genders
    //{
    //    Woman = 1, // will start from 1
    //    Man = 2 // will get the value 2 and other elements will continue to get the next values after 2
    //}
    // Way 3:
    public enum Genders
    {
        Woman = 1, // will start from 1
        Man // will automatically get the next value 2 and other elements will continue to get the next values after 2
    }
}
