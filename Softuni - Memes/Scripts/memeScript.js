//  Download function
var canvas = document.getElementById('canvas'),
    ctx = canvas.getContext('2d');
function downloadCanvas(link, canvasId, filename) {
    link.href = document.getElementById(canvasId).toDataURL();
    link.download = filename;
}
document.getElementById('download').addEventListener('click', function() {
    downloadCanvas(this, 'canvas', 'test.png');
}, false);
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

// Draw in canvas tag

function draw(text1,text2,size1,size2,img) {
    /* draw something */
    if(img == undefined){
        // TODO: What happens when there is no img uploaded !
    } else {
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
        ctx.fillStyle = '#fff';
        ctx.font = size1+'px impact';
        wrapTopText (ctx, text1, canvas.width/2, size1 - 5, canvas.width - 60, size1-10);
        ctx.font = size2+'px impact';
        ctx.font = 'impact';
        wrapBottomText(ctx, text2, canvas.width / 2, canvas.height-10, canvas.width - 60, size2 - 10);

    }
}
$('.tb').each(function() {
    var elem = $(this);
    elem.data('oldVal', elem.val());
    elem.bind("propertychange change click keyup input paste", function(event){
        if (elem.data('oldVal') != elem.val()) {
            elem.data('oldVal', elem.val());

            // Do action
            draw($('#text-box1').val(),$('#text-box2').val(),$('#size-font1').val(),$('#size-font2').val(),image);
        }
    });
});
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            image = new Image();
            image.src = e.target.result;
            draw($('#text-box1').val(),$('#text-box2').val(),$('#size-font1').val(),$('#size-font2').val(),image);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#img").change(function(){
    readURL(this);
});
function wrapTopText (context, text, x, y, maxWidth, lineHeight) {

    var words = text.split(''),
        line = '',
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
            words.splice(i + 1, 0,  words[i].substr(test.length))
            words[i] = test;
        }

        test = line + words[i] + '';
        metrics = context.measureText(test);

        if (metrics.width > maxWidth && i > 0) {
            context.textAlign="center";
            context.fillText(line, x, y);
            context.strokeText(line,x,y);
            line = words[i] + '';
            y += lineHeight;
            lineCount++;
        }
        else {
            line = test;
        }
    }
    context.textAlign="center";
    context.fillText(line, x, y);
    context.strokeText(line,x,y);
}
function wrapBottomText (context, text, x, y, maxWidth, lineHeight) {
    
    var words = text.split(''),
        line = '',
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
            words.splice(i + 1, 0,  words[i].substr(test.length))
            words[i] = test;
        }

        test = line + words[i] + '';
        metrics = context.measureText(test);
        if (metrics.width > maxWidth && i > 0) {

            context.fillText(line, x, y);
            context.strokeText(line, x, y);
            line = words[i] + '';
            y += lineHeight;
            lineCount++;
        }
        else {
            line = test;
        }
        console.log(line);
    }
    context.fillText(line, x, y);
    context.strokeText(line, x, y);
}

