using System;
using System.Collections.Generic;

namespace MapBuilder.App
{
    public class MapBuilder
    {
        private SignStep _currentOfficeManagerPosition = new()
        {
            Floor = 0,
            Section = 1
        };

        public IEnumerable<string> BuildRouteMap(IEnumerable<SignStep> signatureMap)
        {
            var routeMap = new List<string>();

            foreach (var signature in signatureMap)
            {
                string elevatorType = GetElevatorType(signature.Floor);
    
                var timeByElevator = GetTimeByElevator(signature); 

                var timeByStairs = GetTimeByStairs(signature.Floor);

                routeMap.Add(GetFastestRoute(timeByStairs, timeByElevator, elevatorType));

                _currentOfficeManagerPosition.Floor = signature.Floor;
                _currentOfficeManagerPosition.Section = signature.Section;
            }

            return routeMap;
        }

        private string GetElevatorType(int floor)
        {
            var result = floor % 2 == 0 ? "E1" : "E2";
            return result;
        }

        private string GetFastestRoute(int timeByStairs, int timeByElevator, string elevatorType)
        {
            var result = timeByStairs <= timeByElevator ? "S" : elevatorType;
            return result;     
        }

        private int GetTimeByStairs(int floor)
        {
          var result = floor > _currentOfficeManagerPosition.Floor ? 
                      (floor - _currentOfficeManagerPosition.Floor) * 2 : 
                      (_currentOfficeManagerPosition.Floor - floor) * 2;

          return result;
        }

        private int GetTimeByElevator(SignStep signature)
        {
          var result = 1; //include time for waiting an elevator
          
          if(signature.Section != _currentOfficeManagerPosition.Section)
             result += 1;

          result += signature.Floor > _currentOfficeManagerPosition.Floor ?
                  (signature.Floor - _currentOfficeManagerPosition.Floor) :
                  (_currentOfficeManagerPosition.Floor - signature.Floor);

          return result;
        }
    }
}