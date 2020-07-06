namespace PolicyService.Domain.Entities
{
    public class TextAnswer : Answer<string>
    {
        public TextAnswer(string questionCode, string answer)
        {
            this.AnswerValue = answer;
            this.QuestionCode = questionCode;
        }
    }
}
