using Domain.Enum;

namespace Domain.Data
{
    public class ActionResult
    {
        private ActionResultType _actionState;
        private string _resultText;
        private int _resultCode;

        public ActionResultType ActionState => _actionState;
        public string ResultText => _resultText;
        public int ResultCode => _resultCode;

        public ActionResult(ActionResultType actionState, string resultText, int resultCode)
        {
            _actionState = actionState;
            _resultText = resultText;
            _resultCode = resultCode;
        }

        public void ChangeResult(ActionResultType actionState, string resultText, int resultCode)
        {
            _actionState = actionState;
            _resultText = resultText;
            _resultCode = resultCode;
        }
    }
}