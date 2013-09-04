<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Fanex.Ares.Website.Test.Default" %>

<!DOCTYPE />
<html ng-app>
<head runat="server">
    <title></title>
    <script src="http://ajax.googleapis.com/ajax/libs/angularjs/1.2.0rc1/angular.min.js"></script>
</head>
<body ng-controller="PhoneListCtrl">
    <ul>
        <li ng-repeat="phone in phones">
            {{phone.name}}
            <p>
                {{phone.snippet}}</p>
        </li>
    </ul>

    
<script language="javascript" type="text/javascript">
    function PhoneListCtrl($scope) {
        $scope.phones = [
            { "name": "Nexus S",
                "snippet": "Fast just got faster with Nexus S."
            },
            { "name": "Motorola XOOM™ with Wi-Fi",
                "snippet": "The Next, Next Generation tablet."
            },
            { "name": "MOTOROLA XOOM™",
                "snippet": "The Next, Next Generation tablet."
            }
            ];
    }
</script>
</body>

</html>


