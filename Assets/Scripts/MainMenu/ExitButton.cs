﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MenuButton
{
    void OnMouseDown()
    {
        Application.Quit();
    }
}
