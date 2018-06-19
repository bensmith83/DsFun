# DsFun
DsFun seeks to implement Microsoft Directory Services functions in C#. Much borrowed with love from pinvoke.net

This is mostly just to provide a quick implementation to show how it can be done in C#. This is not full featured. At some point I'll change it to take user input for the DC name/IP, but for now it defaults to 192.168.0.180.

There are already great tools to do this with Powershell and various other methods, but I wasn't aware of anything in C#. If there's interest, I'll add features, but really I just wanted to see how easy it was.

## Functions
Implemented thus far are:
1. DsGetDcName - returns the name of a DC in the domain. takes optional DC name and/or domain name
2. DsEnumerateDomainTrusts - returns trust data for the domain. 
4. DsGetDCSiteCoverage - returns site names for all sites covered by DC. takes optional DC name
23. DsGetDomainControllerInfo - retrieves information about the domain controller. takes a DC name and an optional domain name
24. DsBindWithCred

More to come...

## Usage
```
DsFun.exe
<enter an option from above>
<follow the prompts>
<this is a really simple program>
```

## Example
DsGetDcName & DsEnumerateDomainTrusts
![DsFun Options 1 & 2](/img/dsfun_opt_1_2.png?raw=true "DsFun Options 1 & 2")
DsGetDCSiteCoverage & DsGetDomainControllerInfo
![DsFun Options 4 & 23](/img/dsfun_opt_1_2.png?raw=true "DsFun Options 4 & 23")
