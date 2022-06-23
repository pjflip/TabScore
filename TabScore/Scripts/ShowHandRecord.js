setNSEW(model.PerspectiveDirection);

function setNSEW(direction) {
    var btn = document.getElementById('btnNorth');
    if (btn) btn.className = "btn btn-secondary m-1";
    btn = document.getElementById('btnSouth')
    if (btn) btn.className = "btn btn-secondary m-1";
    btn = document.getElementById('btnEast');
    if (btn) btn.className = "btn btn-secondary m-1";
    btn = document.getElementById('btnWest')
    if (btn) btn.className = "btn btn-secondary m-1";
    if (direction == 'North') {
        btn = document.getElementById('btnNorth');
        if (btn) btn.className = "btn btn-warning m-1";
        document.getElementById('TS').innerHTML = model.SouthSpades;
        document.getElementById('TH').innerHTML = model.SouthHearts;
        document.getElementById('TD').innerHTML = model.SouthDiamonds;
        document.getElementById('TC').innerHTML = model.SouthClubs;
        document.getElementById('LS').innerHTML = model.EastSpades;
        document.getElementById('LH').innerHTML = model.EastHearts;
        document.getElementById('LD').innerHTML = model.EastDiamonds;
        document.getElementById('LC').innerHTML = model.EastClubs;
        document.getElementById('RS').innerHTML = model.WestSpades;
        document.getElementById('RH').innerHTML = model.WestHearts;
        document.getElementById('RD').innerHTML = model.WestDiamonds;
        document.getElementById('RC').innerHTML = model.WestClubs;
        document.getElementById('BS').innerHTML = model.NorthSpades;
        document.getElementById('BH').innerHTML = model.NorthHearts;
        document.getElementById('BD').innerHTML = model.NorthDiamonds;
        document.getElementById('BC').innerHTML = model.NorthClubs;
        if (model.EvalNorthNT == "###") {
            document.getElementById('T').innerHTML = "S";
            document.getElementById('L').innerHTML = "E";
            document.getElementById('R').innerHTML = "W";
            document.getElementById('B').innerHTML = "N";
        }
        else {
            document.getElementById('T').innerHTML = "S (" + model.HCPSouth + ")";
            document.getElementById('L').innerHTML = "E (" + model.HCPEast + ")";
            document.getElementById('R').innerHTML = "(" + model.HCPWest + ") W";
            document.getElementById('B').innerHTML = "N (" + model.HCPNorth + ")";
        }
    }
    else if (direction == 'South') {
        btn = document.getElementById('btnSouth');
        if (btn) btn.className = "btn btn-warning m-1";
        document.getElementById('TS').innerHTML = model.NorthSpades;
        document.getElementById('TH').innerHTML = model.NorthHearts;
        document.getElementById('TD').innerHTML = model.NorthDiamonds;
        document.getElementById('TC').innerHTML = model.NorthClubs;
        document.getElementById('LS').innerHTML = model.WestSpades;
        document.getElementById('LH').innerHTML = model.WestHearts;
        document.getElementById('LD').innerHTML = model.WestDiamonds;
        document.getElementById('LC').innerHTML = model.WestClubs;
        document.getElementById('RS').innerHTML = model.EastSpades;
        document.getElementById('RH').innerHTML = model.EastHearts;
        document.getElementById('RD').innerHTML = model.EastDiamonds;
        document.getElementById('RC').innerHTML = model.EastClubs;
        document.getElementById('BS').innerHTML = model.SouthSpades;
        document.getElementById('BH').innerHTML = model.SouthHearts;
        document.getElementById('BD').innerHTML = model.SouthDiamonds;
        document.getElementById('BC').innerHTML = model.SouthClubs;
        if (model.EvalNorthNT == "###") {
            document.getElementById('T').innerHTML = "N";
            document.getElementById('L').innerHTML = "W";
            document.getElementById('R').innerHTML = "E";
            document.getElementById('B').innerHTML = "S";
        }
        else {
            document.getElementById('T').innerHTML = "N (" + model.HCPNorth + ")";
            document.getElementById('L').innerHTML = "W (" + model.HCPWest + ")";
            document.getElementById('R').innerHTML = "(" + model.HCPEast + ") E";
            document.getElementById('B').innerHTML = "S (" + model.HCPSouth + ")";
        }
    }
    else if (direction == 'East')
    {
        btn = document.getElementById('btnEast');
        if (btn) btn.className = "btn btn-warning m-1";
        document.getElementById('TS').innerHTML = model.WestSpades;
        document.getElementById('TH').innerHTML = model.WestHearts;
        document.getElementById('TD').innerHTML = model.WestDiamonds;
        document.getElementById('TC').innerHTML = model.WestClubs;
        document.getElementById('LS').innerHTML = model.SouthSpades;
        document.getElementById('LH').innerHTML = model.SouthHearts;
        document.getElementById('LD').innerHTML = model.SouthDiamonds;
        document.getElementById('LC').innerHTML = model.SouthClubs;
        document.getElementById('RS').innerHTML = model.NorthSpades;
        document.getElementById('RH').innerHTML = model.NorthHearts;
        document.getElementById('RD').innerHTML = model.NorthDiamonds;
        document.getElementById('RC').innerHTML = model.NorthClubs;
        document.getElementById('BS').innerHTML = model.EastSpades;
        document.getElementById('BH').innerHTML = model.EastHearts;
        document.getElementById('BD').innerHTML = model.EastDiamonds;
        document.getElementById('BC').innerHTML = model.EastClubs;
        if (model.EvalNorthNT == "###") {
            document.getElementById('T').innerHTML = "W";
            document.getElementById('L').innerHTML = "S";
            document.getElementById('R').innerHTML = "N";
            document.getElementById('B').innerHTML = "E";
        }
        else {
            document.getElementById('T').innerHTML = "W (" + model.HCPWest + ")";
            document.getElementById('L').innerHTML = "S (" + model.HCPSouth + ")";
            document.getElementById('R').innerHTML = "(" + model.HCPNorth + ") N";
            document.getElementById('B').innerHTML = "E (" + model.HCPEast + ")";
        }
    }
    else if (direction == 'West') {
        btn = document.getElementById('btnWest')
        if (btn) btn.className = "btn btn-warning m-1";
        document.getElementById('TS').innerHTML = model.EastSpades;
        document.getElementById('TH').innerHTML = model.EastHearts;
        document.getElementById('TD').innerHTML = model.EastDiamonds;
        document.getElementById('TC').innerHTML = model.EastClubs;
        document.getElementById('LS').innerHTML = model.NorthSpades;
        document.getElementById('LH').innerHTML = model.NorthHearts;
        document.getElementById('LD').innerHTML = model.NorthDiamonds;
        document.getElementById('LC').innerHTML = model.NorthClubs;
        document.getElementById('RS').innerHTML = model.SouthSpades;
        document.getElementById('RH').innerHTML = model.SouthHearts;
        document.getElementById('RD').innerHTML = model.SouthDiamonds;
        document.getElementById('RC').innerHTML = model.SouthClubs;
        document.getElementById('BS').innerHTML = model.WestSpades;
        document.getElementById('BH').innerHTML = model.WestHearts;
        document.getElementById('BD').innerHTML = model.WestDiamonds;
        document.getElementById('BC').innerHTML = model.WestClubs;
        if (model.EvalNorthNT == "###") {
            document.getElementById('T').innerHTML = "E";
            document.getElementById('L').innerHTML = "N";
            document.getElementById('R').innerHTML = "S";
            document.getElementById('B').innerHTML = "W";
        }
        else {
            document.getElementById('T').innerHTML = "E (" + model.HCPEast + ")";
            document.getElementById('L').innerHTML = "N (" + model.HCPNorth + ")";
            document.getElementById('R').innerHTML = "(" + model.HCPSouth + ") S";
            document.getElementById('B').innerHTML = "W (" + model.HCPWest + ")";
        }
    }
}

