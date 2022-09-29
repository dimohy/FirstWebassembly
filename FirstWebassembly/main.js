// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './dotnet.js'

const is_browser = typeof window != "undefined";
if (!is_browser) throw new Error(`Expected to be running in a browser`);

const { setModuleImports, getAssemblyExports, getConfig, runMainAndExit } = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .create();

const canvas = document.getElementById("canvas");
const context = canvas.getContext("2d");

setModuleImports("main.js", {
    window: {
        location: {
            href: () => globalThis.window.location.href
        }
    },

    requestAnimationFrame: (callback) => window.requestAnimationFrame(callback),

    clearCanvas: () => {
        //context.fillstyle = "white";
        //context.fillRect(0, 0, canvas.width, canvas.height);
        context.clearRect(0, 0, canvas.width, canvas.height);
    },

    drawCircle: (x, y, radius, color) => {
        context.beginPath();
        context.fillStyle = color;
        context.arc(x, y, radius, 0, 2 * Math.PI, false);
        context.fill();
    }
});

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);
// const text = exports.MyClass.Greeting();
// console.log(text);

//document.getElementById("out").innerHTML = `${text}`;
await runMainAndExit(config.mainAssemblyName, ["dotnet", "is", "great!"]);