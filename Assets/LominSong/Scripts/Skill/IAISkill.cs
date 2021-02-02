using System.Collections;
using System.Collections.Generic;

public interface IAISkill
{
    void OnEnter();
    void Inference();
    bool Decision();
    bool Action();
}
