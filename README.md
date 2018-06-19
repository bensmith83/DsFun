# DsFun
DsFun seeks to implement Microsoft Directory Services functions in C#. Much borrowed with love from pinvoke.net

This is mostly just to provide a quick implementation to show how it can be done in C#. This is not full featured. At some point I'll change it to take user input for the DC name/IP, but for now it defaults to 192.168.0.180.

## Functions
Implemented thus far are:
1. DsGetDcName
2. DsEnumerateDomainTrusts
4. DsGetDCSiteCoverage
23. DsGetDomainControllerInfo
24. DsBindWithCred

More to come...

## Usage
```
DsFun.exe
<enter an option from above>
<follow the prompts>
<this is a really simple program>
```
