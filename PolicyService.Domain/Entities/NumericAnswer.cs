namespace PolicyService.Domain.Entities
{
    public class NumericAnswer : Answer<decimal>
    {
        public NumericAnswer(string questionCode, decimal answer)
        {
            this.AnswerValue = answer;
            this.QuestionCode = questionCode;
        }
    }
}
