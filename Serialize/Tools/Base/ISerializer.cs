using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Serialize.Tools.Base
{
    public interface ISerializer
    {
        object Serialize<_T>(_T dataToSerialize);
        _T Deserialize<_T>(object serializedData);
    }
}
