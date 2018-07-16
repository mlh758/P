﻿using System.Collections.Generic;
using Microsoft.Pc.TypeChecker.AST.Declarations;
using Microsoft.Pc.TypeChecker.AST.States;
using Microsoft.Pc.TypeChecker.Types;

namespace Microsoft.Pc.Backend.Prt
{

    public class CompilationContext : CompilationContextBase
    {
        private readonly Dictionary<Interface, int> interfaceNumbering = new Dictionary<Interface, int>();
        private readonly Dictionary<Machine, int> machineNumbering = new Dictionary<Machine, int>();
        private readonly Dictionary<PEvent, int> eventNumbering = new Dictionary<PEvent, int>();
        private readonly Dictionary<Machine, Dictionary<State, int>> stateNumbering = new Dictionary<Machine, Dictionary<State, int>>();

        private readonly ValueInternmentManager<bool> registeredBools;
        private readonly ValueInternmentManager<double> registeredFloats;
        private readonly ValueInternmentManager<int> registeredInts;

        public CompilationContext(ICompilationJob job) : base(job)
        {
            Names = new PrtNameManager($"P_");
            HeaderFileName = $"{job.ProjectName}.h";
            SourceFileName = $"{job.ProjectName}.c";
            registeredInts = new ValueInternmentManager<int>(Names);
            registeredFloats = new ValueInternmentManager<double>(Names);
            registeredBools = new ValueInternmentManager<bool>(Names);
        }

        public PrtNameManager Names { get; }

        public string HeaderFileName { get; }
        public string SourceFileName { get; }
        public IEnumerable<PLanguageType> UsedTypes => Names.UsedTypes;
        public HashSet<PLanguageType> WrittenTypes { get; } = new HashSet<PLanguageType>();

        #region Numbering helpers

        private static int GetOrAddNumber<T>(IDictionary<T, int> dict, T declaration)
        {
            if (dict.TryGetValue(declaration, out int number))
            {
                return number;
            }

            number = dict.Count;
            dict.Add(declaration, number);
            return number;
        }

        public int GetDeclNumber(Interface pInterface)
        {
            return GetOrAddNumber(interfaceNumbering, pInterface);
        }

        public int GetDeclNumber(Machine machine)
        {
            return GetOrAddNumber(machineNumbering, machine);
        }

        public int GetDeclNumber(PEvent ev)
        {
            return GetOrAddNumber(eventNumbering, ev);
        }

        public int GetDeclNumber(State state)
        {
            Machine machine = state.OwningMachine;
            if (!stateNumbering.TryGetValue(machine, out var internalNumbering))
            {
                internalNumbering = new Dictionary<State, int>();
                stateNumbering.Add(machine, internalNumbering);
            }

            return GetOrAddNumber(internalNumbering, state);
        }

        #endregion

        public string RegisterLiteral(Function function, int value)
        {
            return registeredInts.RegisterValue(function, value);
        }

        public IEnumerable<KeyValuePair<int, string>> GetRegisteredIntLiterals(Function function)
        {
            return registeredInts.GetValues(function);
        }

        internal string RegisterLiteral(Function function, double value)
        {
            return registeredFloats.RegisterValue(function, value);
        }

        public IEnumerable<KeyValuePair<double, string>> GetRegisteredFloatLiterals(Function function)
        {
            return registeredFloats.GetValues(function);
        }

        public string RegisterLiteral(Function function, bool value)
        {
            return registeredBools.RegisterValue(function, value);
        }

        public IEnumerable<KeyValuePair<bool, string>> GetRegisteredBoolLiterals(Function function)
        {
            return registeredBools.GetValues(function);
        }
    }
}
