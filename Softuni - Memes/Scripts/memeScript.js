var canvas = document.getElementById('canvas'),
    ctx = canvas.getContext('2d');

//snap dial box open/close
var modal = document.getElementById('myModal');

var btn = document.getElementById("myBtn");

var span = document.getElementsByClassName("close")[0];

btn.onclick = function () {
    modal.style.display = "block";
}

span.onclick = function () {
    modal.style.display = "none";
    Webcam.reset();
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
// snap import in canvas
function import_image() {
    var childs = document.getElementById('my_result').childNodes;
    var img = childs[0].src;
    var image = new Image();
    image.src = img;
    $('#text-box1').val('');
    $('#text-box2').val('');
    childs[0].src = "";
    modal.style.display = "none";
    draw($('#text-box1').val(), $('#text-box2').val(), $('#size-font1').val(), $('#size-font2').val(), image);
    Webcam.reset();
}


window.onload = (function () {
    ctx.beginPath();
    ctx.rect(0,0, 600, 500);
    ctx.fillStyle = "white";
    ctx.fill();
    ctx.fillStyle = '#224364';
    ctx.font = 70 + 'px impact';
    ctx.textAlign = "center";
    ctx.lineWidth = 2;
    wrapTopText(ctx, "Upload image or choose from given below", canvas.width / 2, canvas.height / 3, canvas.width - 60, 70);
    document.getElementById('preview').setAttribute("src", document.getElementById('canvas').toDataURL());
})
// Draw in canvas tag
lines = 0;
function draw(text1, text2, size1, size2, img) {
    /* draw something */
    if (img != undefined) {
        $('#aside').removeAttr('hidden');
        ctx.shadowBlur = 10;
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
        ctx.fillStyle = '#fff';
        ctx.font = size1 + 'px impact';
        wrapTopText(ctx, text1, canvas.width / 2, size1 - 5, canvas.width - 60, size1 - 10);
        ctx.font = size2 + 'px impact';
        ctx.font = 'impact';
        wrapBottomText(ctx, text2, canvas.width / 2, canvas.height - lines * (size2 - 10) - 10, canvas.width - 60, size2 - 10);
        
        document.getElementById('preview').setAttribute("src", document.getElementById('canvas').toDataURL());
    }
}

// Handle changes in text
$('.tb').each(function () {
    var elem = $(this);
    elem.data('oldVal', elem.val());
    elem.bind("propertychange change click keyup input paste", function (event) {
        if (elem.data('oldVal') != elem.val()) {
            elem.data('oldVal', elem.val());

            // Do action
            draw($('#text-box1').val(), $('#text-box2').val(), $('#size-font1').val(), $('#size-font2').val(), image);
        }
    });
});

// Handle change in template
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            image = new Image();
            image.src = e.target.result;
            draw($('#text-box1').val(), $('#text-box2').val(), $('#size-font1').val(), $('#size-font2').val(), image);
        }
        reader.readAsDataURL(input.files[0]);
    }
}
$("#file").change(function () {
    readURL(this);
    $('#text-box1').val('');
    $('#text-box2').val('');
});
function reply_click(clicked_id) {
    $('#text-box1').val('');
    $('#text-box2').val('');
    name = clicked_id;
    image = new Image();
    srcimg = '../Images/' + name + '.jpg';
    image.src = srcimg;
    draw($('#text-box1').val(), $('#text-box2').val(), $('#size-font1').val(), $('#size-font2').val(), image);
    $('#img').val(undefined);
}

function toDataUrl(url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.responseType = 'blob';
    xhr.onload = function () {
        var reader = new FileReader();
        reader.onloadend = function () {
            callback(reader.result);
        }
        reader.readAsDataURL(xhr.response);
    };
    xhr.open('GET', url);
    xhr.send();
}
// Positioning of top text 
function wrapTopText(context, text, x, y, maxWidth, lineHeight) {
    var words = text.split(' '),
        line = ' ',
        lineCount = 0,
        i,
        test,
        metrics;
    for (i = 0; i < words.length; i++) {
        test = words[i];
        metrics = context.measureText(test);
        while (metrics.width > maxWidth) {
            // Determine how much of the word will fit
            test = test.substring(0, test.length - 1);
            metrics = context.measureText(test);
        }
        if (words[i] != test) {
            words.splice(i + 1, 0, words[i].substr(test.length))
            words[i] = test;
        }
        test = line + words[i] + ' ';
        metrics = context.measureText(test);
        if (metrics.width > maxWidth && i > 0) {
            context.textAlign = "center";
            context.fillText(line, x, y);
            context.strokeText(line, x, y);
            line = words[i] + ' ';
            y += lineHeight;
            lineCount++;
        }
        else {
            line = test;
        }
    }
    context.textAlign = "center";
    context.fillText(line, x, y);
    context.strokeText(line, x, y);
}
function wrapBottomText(context, text, x, y, maxWidth, lineHeight) {

    var words = text.split(' '),
        line = ' ',
        lineCount = 0,
        i,
        test,
        metrics;

    for (i = 0; i < words.length ; i++) {
        test = words[i];
        metrics = context.measureText(test);
        while (metrics.width > maxWidth) {
            // Determine how much of the word will fit
            test = test.substring(0, test.length - 1);
            metrics = context.measureText(test);
        }
        if (words[i] != test) {
            words.splice(i + 1, 0, words[i].substr(test.length))
            words[i] = test;
        }
        test = line + words[i] + ' ';
        metrics = context.measureText(test);
        if (metrics.width > maxWidth && i > 0) {
            context.textAlign = "center";
            context.fillText(line, x, y);
            context.strokeText(line, x, y);
            line = words[i] + ' ';
            y += lineHeight;
            lineCount++;
        }
        else {
            line = test;
        }
    }
    lines = lineCount;
    context.textAlign = "center";
    context.fillText(line, x, y);
    context.strokeText(line, x, y);
}

$('.choose-button').click(function (

    ) {
    $('html, body').animate({ scrollTop: 0 }, 800);
    return false;
});
// Download link
function downloadCanvas(link, canvasId, filename) {
    link.href = document.getElementById(canvasId).toDataURL();
    link.download = filename;
}
document.getElementById('download').addEventListener('click', function () {
    downloadCanvas(this, 'canvas', 'test.png');
}, false);

// Save memes in db
document.getElementById('create-button').addEventListener('click', function () {
    let baseStr = document.getElementById('canvas').toDataURL();
    let tokens = baseStr.split(',');
    $.ajax({
        type: "POST",
        url: "/Image/Create",
        data: { base64: tokens[1] },
        success: function (data) {
            window.location = "/Image/Index";
        }
    });
})

