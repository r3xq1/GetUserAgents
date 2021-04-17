# GetUserAgents
Граббер UserAgent'ов через реестр.

Список браузеров с которых собираем агенты:
```csharp
string[] browsers = new string[]
{ 
   "Chrome", "Firefox", "Opera", "Edge", 
   "Internet Explorer", "Waterfox", "Dragon", 
   "Palemoon", "Seamonkey" 
};
```
Вызов метода для сбора:  `UserAgent.GetData(GlobalPaths.AgentsLog);`
