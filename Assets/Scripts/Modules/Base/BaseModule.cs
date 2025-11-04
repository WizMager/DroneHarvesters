using Views;

namespace Modules.Base
{
    public class BaseModule : IBaseModule
    {
        private BaseView _redBase;
        private BaseView _blueBase;

        public BaseModule(BaseView redBase, BaseView blueBase)
        {
            _redBase = redBase;
            _blueBase = blueBase;
        }
    }
}