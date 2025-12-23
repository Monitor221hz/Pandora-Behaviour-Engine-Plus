// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml.Linq;
using Pandora.Models;

namespace PandoraTests;

public static class Resources
{
    public static readonly DirectoryInfo TemplateDirectory = new(
        Path.Combine(Environment.CurrentDirectory, "Pandora_Engine", "Skyrim", "Template")
    );
    public static readonly DirectoryInfo OutputDirectory = new(
        Path.Combine(Environment.CurrentDirectory, "Output")
    );
    public static readonly DirectoryInfo CurrentDirectory = new(Environment.CurrentDirectory);
    public static readonly DirectoryInfo DataDirectory = new(
        Path.Combine(Environment.CurrentDirectory, "Data")
    );

    public static readonly DirectoryInfo OutputMeshDirectory = new DirectoryInfo(
        Path.Combine(Resources.OutputDirectory.FullName, "meshes")
    );

    public static XElement TestElement =>
        new(
            "hkobject",
            new XAttribute("name", "#0069"),
            new XElement(
                "hkparam",
                new XAttribute("name", "variableBindingSet"),
                new XText("#0420")
            ),
            new XElement("hkparam", new XAttribute("name", "userData"), new XText("0")),
            new XElement("hkparam", new XAttribute("name", "name"), new XText("TestNode")),
            new XElement(
                "hkparam",
                new XAttribute("name", "generators"),
                new XAttribute("numelements", "5"),
                new XText("#0001\n#0002\n#0003\n#0004\n#0005")
            )
        );

    static Resources()
    {
        OutputDirectory.Create();
        TemplateDirectory.Create();
        DataDirectory.Create();
        OutputMeshDirectory.Create();
    }
}
