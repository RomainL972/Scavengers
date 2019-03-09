/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

	public GameObject gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.instance == null)
        	Instantiate(gameManager);
    }
}