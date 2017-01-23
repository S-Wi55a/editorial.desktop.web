var gulp = require('gulp'),
    awspublish = require('gulp-awspublish'),
    argv = require('yargs').argv;

var aws = {
    "key": argv.awsKey,
    "secret": argv.awsSecret,
    "bucket": argv.awsBucket,
    "region": "ap-southeast-2"
}

var configs = {
    deploySrc: './dist'
};

gulp.task('upload-s3',
    function () {
        var publisher = awspublish.create({
            params: {
                Bucket: aws.bucket
            },
            region: aws.region,
            accessKeyId: aws.key,
            secretAccessKey: aws.secret
        });

        var headers = {
            'Cache-Control': 'max-age=315360000, public'
        };

        gulp.src(configs.deploySrc + '/**')
            .pipe(publisher.publish(headers))
            .pipe(publisher.cache())
            //.pipe(publisher.sync('carsales/retail/staticpages'))
            .pipe(awspublish.reporter());
    });
