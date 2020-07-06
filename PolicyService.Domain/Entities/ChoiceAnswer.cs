namespace PolicyService.Domain.Entities
{
    public class ChoiceAnswer : Answer<string>
    {
        public ChoiceAnswer(string questionCode, string answer)
        {
            this.AnswerValue = answer;
            this.QuestionCode = questionCode;
        }
    }
}
