using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Pandora.Xml
{
    public class XParticle // not struct because content can be > 16 bytes
    {

        public XObject Content { get; }
        public string StringData { get; set; } = string.Empty;
        private Func<XObject,XParticle, bool> XAction { get; set; } = (obj, xp) => true;

        public XParticle(XObject cont)
        {
            Content = cont;
        }
        public XParticle(XObject cont, string stringData)
        {
            Content = cont;
            this.StringData = stringData;
        }

        public XParticle(XObject cont, Func<XObject,XParticle, bool> func)
        {
            Content = cont;
            XAction = func;
        }
        public XParticle(XObject cont, Func<XObject, XParticle, bool> func, string stringData)
        {
            Content = cont;
            XAction = func;
            this.StringData = stringData;
        }
        public void SetWork(Func<XObject, XParticle, bool> func) => XAction = func;
        
        public async Task<bool> ExecuteAsync(XObject target) => await Task.Run(() => XAction(target, this));
    }
}
