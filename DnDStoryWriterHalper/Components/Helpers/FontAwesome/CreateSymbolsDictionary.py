# namespace DnDStoryWriterHalper.Components.FontAwesome;
# public static class SymbolsContainer
# {
#     public static readonly string[] Symbil =
#     {
#         "",
#         ""
#     };
# }
# public enum Symbols : int
# {
#     Two = 0
# }

def to_camel_case(text):
    if text[0].isdigit():
        text = "Symbol " + text
    if text == "lock":
        text = "Symbol lock"
    s = text.replace("-", " ").replace("_", " ")
    s = s.split()
    if len(text) == 0:
        return text
    return s[0] + ''.join(i.capitalize() for i in s[1:])
    


import json
# use icons.json from metadata from ImaggeAwesome.zip
file = open("icons.json")
data = json.load(file)
file.close()
symbols = data.keys()
enumString = "public enum Symbols : int{"
i = 0
for s in symbols:
    enumString += "    " + to_camel_case(s) + " = 0x" + data[s]["unicode"] + ","
    i += 1

enumString += "}"

open("symbols.cs", "w").write("namespace DnDStoryWriterHalper.Components.Helpers.FontAwesome;\n"+
enumString)
