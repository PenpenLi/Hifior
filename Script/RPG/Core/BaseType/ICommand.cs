using UnityEngine;
using System.Collections;
public interface ICommand
{
    void Do();
}
public interface IUndoableCommand : ICommand
{
    void Undo();
}