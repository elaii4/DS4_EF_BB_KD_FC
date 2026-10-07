using Microsoft.Agents.AI;
using System;
using System.Reflection;
using System.Linq;

namespace Proyecto2_
{
    class Test 
    {
        void Go() 
        {
            var type = typeof(ChatClientAgentSession);
            foreach (var ctor in type.GetConstructors()) {
                Console.WriteLine("Ctor: " + string.Join(", ", ctor.GetParameters().Select(p => p.ParameterType.Name)));
            }
        }
    }
}
