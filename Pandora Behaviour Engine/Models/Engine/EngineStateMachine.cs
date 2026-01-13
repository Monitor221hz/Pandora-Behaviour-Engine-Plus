using Pandora.Enums;
using System;
using System.Reactive.Subjects;

namespace Pandora.Models.Engine;

public sealed class EngineStateMachine : IEngineStateMachine
{
	private readonly BehaviorSubject<EngineState> _state = new(EngineState.Uninitialized);

	public EngineState Current => _state.Value;
	public IObservable<EngineState> Changes => _state;

	public void Transition(EngineState next)
	{
		if (_state.Value != next)
			_state.OnNext(next);
	}
}
