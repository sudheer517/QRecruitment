/// <binding ProjectOpened='watch:less' />
// This file in the main entry point for defining grunt tasks and using grunt plugins.
// Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409

module.exports = function (grunt) {
    grunt.initConfig({
        bower: {
            install: {
                options: {
                    targetDir: "wwwroot/lib",
                    layout: "byComponent",
                    cleanTargetDir: false
                }
            }
        },
        watch: {
            less: {
                files: ["Content/Less/**/*.less", "Content/Less/*.less"], // Source folder for less
                tasks: ["less"],
                options: {
                    livereload: true
                }
            }
        },
        less: {
            dist: {
                options: {
                    paths: ["Less"]
                },
                files: {
                    'wwwroot/css/site.css': ['Content/Less/**/*.less', 'Content/Less/*.less']
                }
            }
        }
    });

    // This command registers the default task which will install bower packages into wwwroot/lib
    grunt.registerTask("default", ["bower:install"]);

    // The following line loads the grunt plugins.
    // This line needs to be at the end of this this file.
    grunt.loadNpmTasks("grunt-bower-task");
    grunt.loadNpmTasks("grunt-contrib-less");
    grunt.loadNpmTasks("grunt-contrib-watch");
};