# xls2json-unity
Unity engine integration of xls2json tool; provides for automated import of XLS data

- Depends on xls2json: https://github.com/ktagawa/xls2json
- Depends on xlrd: https://github.com/python-excel/xlrd
- Depends on litjson: https://github.com/lbv/litjson

Check out the notes on the <a href=https://github.com/ktagawa/xls2json>xls2json</a> page for more details on how xls2json works.

This project provides an integration of <a href=https://github.com/ktagawa/xls2json>xls2json</a> to Unity3D (for which xls2json was originally designed), including:

- A <a href = https://github.com/lbv/litjson>LitJson</a> loader
- A basic implementation of the base Data.cs type
- Sample XLS/JSON/POCO sources
- Asset Preprocessor bindings to trigger import on modification
- UnityEditor wrapping import settings, etc.

