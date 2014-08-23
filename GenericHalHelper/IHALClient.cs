using GenericHalHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericHalHelper
{
    public interface IHalClient
    {
        HalObject Get(string relativePath);
        HalObject Get(string relativePath, object uriParamters);
        HalObject Get(Link linkToNextState);

        HalObject Get(Link linkToNextState, object uriParameters);
    }
}
