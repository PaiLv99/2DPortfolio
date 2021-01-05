using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecevier
{
    void Handle(BaseTask message);
}
