namespace PolicyService.Domain.Entities
{
    using System;
    
    using MicroservicesPOC.Shared.Domain;
    using MicroservicesPOC.Shared.Domain.Enums;

    public abstract class Answer : Entity<Guid>
    {
        //TODO: fix question code to Id
        public string QuestionCode { get; protected set; }

        public abstract object GetAnswerValue();

        public static Answer Create(QuestionType questionType, string questionCode, object answerValue) =>
            questionType switch
            {
                QuestionType.Numeric => new NumericAnswer(questionCode, (decimal)answerValue),
                QuestionType.Text => new TextAnswer(questionCode, (string)answerValue),
                QuestionType.Choice => new ChoiceAnswer(questionCode, (string)answerValue),
                _ => throw new ArgumentException(),
            };
    }

    public abstract class Answer<T> : Answer
    {
        public T AnswerValue { get; protected set; }

        public override object GetAnswerValue() => this.AnswerValue;
    }
}
