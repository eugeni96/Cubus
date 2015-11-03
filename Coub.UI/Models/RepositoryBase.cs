using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adam.Tools;
using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Records;
using Adam.Core.Search;

namespace Coub.UI.Models
{
    public class RepositoryBase
    {
        private Application app;

        public RepositoryBase(Application app)
        {
            this.app = app;
        }

        public void GetCoubListPath()
        {
            Classification clf = new Classification(app);

            if (clf.TryLoad(new SearchExpression("Name = 'Coub'"))==TryLoadResult.Success)
            {
                Record record = new Record(app);
                record.Load(new SearchExpression());
            }

        }

    }
}