using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace LiteDevelop.Essentials.FormsDesigner.Services
{
    public class EventBindingService : System.ComponentModel.Design.EventBindingService 
    {
        private IServiceProvider _serviceProvider;

        public EventBindingService(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override string CreateUniqueMethodName(IComponent component, EventDescriptor e)
        {
            return string.Empty;
        }

        protected override ICollection GetCompatibleMethods(EventDescriptor e)
        {
            return new PropertyDescriptorCollection(null);
        }

        protected override bool ShowCode(IComponent component, EventDescriptor e, string methodName)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowCode(int lineNumber)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowCode()
        {
            throw new NotImplementedException();
        }
    }
}
