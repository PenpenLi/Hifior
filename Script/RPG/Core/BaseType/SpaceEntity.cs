using System;

public abstract class ViSpaceEntity : GeographicObject, IGeographicInterface, IDestroyable
{
    //public ViSpaceEntity()
    //{
    //    _spaceNode = ViRefNodeMaker<ViSpaceEntity>.Alloc(this);
    //}

    //public virtual void Destroy()
    //{
    //    ViRefNodeMaker<ViSpaceEntity>.DeAlloc(ref _spaceNode);
    //}

    //public ViEntityID ID { get { return _ID; } set { _ID = value; } }
    //public ViSpace Space { get { return _space; } set { _space = value; } }
    //public ViSpaceChunk SpaceChunk { get { return _spaceChunk; } set { _spaceChunk = value; } }
    //public ViAccumulateCount AoIAccumulateCount { get { return _AoIAccumulate; } }
    //public ViRefNode2<ViSpaceEntity> SpaceNode { get { return _spaceNode; } }
    //public ViRefList2<ViObserverNode> ObserverList { get { return _observerList; } }

    //ViEntityID _ID;
    //ViSpace _space = null;
    //ViSpaceChunk _spaceChunk = null;
    //ViRefNode2<ViSpaceEntity> _spaceNode = null;
    //ViRefList2<ViObserverNode> _observerList = new ViRefList2<ViObserverNode>();

    //ViAccumulateCount _AoIAccumulate = new ViAccumulateCount();
    public void Destroy()
    {
        throw new NotImplementedException();
    }
}