using Pandora.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pandora.Models.Engine;

public interface IEngineStateMachine
{
	EngineState Current { get; }
	IObservable<EngineState> Changes { get; }
	void Transition(EngineState next);
}
