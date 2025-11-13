pipeline {
    agent any
    
    environment {
        DOTNET_CLI_HOME = "/tmp/dotnet"
        DOTNET_ROOT = "/usr/bin/dotnet"
        PROJECT_NAME = "TodoBlazorApp"
        SOLUTION_FILE = "TodoBlazorApp.sln"
        API_PROJECT = "src/Presentation/TodoBlazorApp.API/TodoBlazorApp.API.csproj"
        UI_PROJECT = "src/Presentation/TodoBlazorApp.BlazorUI/TodoBlazorApp.BlazorUI.csproj"
        BUILD_VERSION = "${env.BUILD_NUMBER}"
        GIT_COMMIT_SHORT = sh(script: "git rev-parse --short HEAD", returnStdout: true).trim()
    }
    
    options {
        buildDiscarder(logRotator(numToKeepStr: '10'))
        timestamps()
        timeout(time: 30, unit: 'MINUTES')
    }
    
    stages {
        stage('Checkout') {
            steps {
                echo '🔍 استنساخ الكود من Git...'
                checkout scm
                sh 'git log -1 --pretty=format:"%h - %an: %s"'
            }
        }
        
        stage('Environment Info') {
            steps {
                echo '📋 معلومات البيئة'
                sh '''
                    echo "Build Number: ${BUILD_NUMBER}"
                    echo "Git Commit: ${GIT_COMMIT_SHORT}"
                    echo "Branch: ${GIT_BRANCH}"
                    dotnet --version
                    dotnet --list-sdks
                '''
            }
        }
        
        stage('Restore Dependencies') {
            steps {
                echo '📦 استعادة الحزم...'
                sh 'dotnet restore ${SOLUTION_FILE}'
            }
        }
        
        stage('Build Solution') {
            steps {
                echo '🔨 بناء المشروع...'
                sh 'dotnet build ${SOLUTION_FILE} --configuration Release --no-restore'
            }
        }
        
        stage('Run Tests') {
            steps {
                echo '🧪 تشغيل الاختبارات...'
                script {
                    def testProjects = sh(
                        script: 'find tests -name "*.csproj" 2>/dev/null || true',
                        returnStdout: true
                    ).trim()
                    
                    if (testProjects) {
                        sh '''
                            dotnet test ${SOLUTION_FILE} \
                                --configuration Release \
                                --no-build \
                                --verbosity normal \
                                --logger "trx;LogFileName=test-results.trx" \
                                --collect:"XPlat Code Coverage"
                        '''
                    } else {
                        echo '⚠️ لا توجد اختبارات'
                    }
                }
            }
            post {
                always {
                    script {
                        if (fileExists('**/test-results.trx')) {
                            mstest testResultsFile: '**/test-results.trx'
                        }
                    }
                }
            }
        }
        
        stage('Code Analysis') {
            steps {
                echo '🔍 تحليل الكود...'
                sh '''
                    echo "Running code analysis..."
                    # يمكن إضافة أدوات تحليل الكود هنا مثل SonarQube
                '''
            }
        }
        
        stage('Publish API') {
            steps {
                echo '📦 نشر API...'
                sh '''
                    dotnet publish ${API_PROJECT} \
                        -c Release \
                        -o ${WORKSPACE}/publish/api \
                        --no-build \
                        /p:Version=1.0.${BUILD_NUMBER}
                '''
            }
        }
        
        stage('Publish Blazor UI') {
            steps {
                echo '📦 نشر Blazor UI...'
                sh '''
                    dotnet publish ${UI_PROJECT} \
                        -c Release \
                        -o ${WORKSPACE}/publish/ui \
                        --no-build \
                        /p:Version=1.0.${BUILD_NUMBER}
                '''
            }
        }
        
        stage('Create Database') {
            steps {
                echo '🗄️ إنشاء قاعدة البيانات...'
                script {
                    dir("${WORKSPACE}/publish/api") {
                        sh '''
                            if [ -f TodoBlazorApp.API.dll ]; then
                                echo "Database will be created on first run"
                            fi
                        '''
                    }
                }
            }
        }
        
        stage('Archive Artifacts') {
            steps {
                echo '📚 أرشفة الملفات...'
                archiveArtifacts artifacts: 'publish/**/*', 
                                fingerprint: true,
                                allowEmptyArchive: false
            }
        }
        
        stage('Create Release Package') {
            steps {
                echo '📦 إنشاء حزمة الإصدار...'
                sh '''
                    cd ${WORKSPACE}/publish
                    tar -czf ${PROJECT_NAME}-v1.0.${BUILD_NUMBER}.tar.gz api/ ui/
                    ls -lh *.tar.gz
                '''
                archiveArtifacts artifacts: 'publish/*.tar.gz', fingerprint: true
            }
        }
        
        stage('Deploy to Staging') {
            when {
                branch 'develop'
            }
            steps {
                echo '🚀 النشر على بيئة التجريب...'
                sh '''
                    echo "Deploying to staging environment..."
                    # أضف أوامر النشر على بيئة التجريب
                '''
            }
        }
        
        stage('Deploy to Production') {
            when {
                branch 'main'
            }
            steps {
                echo '🚀 النشر على بيئة الإنتاج...'
                input message: 'هل تريد النشر على الإنتاج؟', ok: 'نشر'
                sh '''
                    echo "Deploying to production environment..."
                    # أضف أوامر النشر على بيئة الإنتاج
                '''
            }
        }
    }
    
    post {
        success {
            echo '✅ البناء والنشر تم بنجاح!'
            script {
                def message = """
                ✅ Build Successful
                Project: ${PROJECT_NAME}
                Build: #${BUILD_NUMBER}
                Commit: ${GIT_COMMIT_SHORT}
                Branch: ${GIT_BRANCH}
                Duration: ${currentBuild.durationString}
                """
                echo message
                
                // إرسال إشعار (يمكن تفعيله لاحقاً)
                // emailext (
                //     subject: "✅ Build Success: ${PROJECT_NAME} #${BUILD_NUMBER}",
                //     body: message,
                //     to: 'hnjm@example.com'
                // )
            }
        }
        
        failure {
            echo '❌ فشل البناء!'
            script {
                def message = """
                ❌ Build Failed
                Project: ${PROJECT_NAME}
                Build: #${BUILD_NUMBER}
                Commit: ${GIT_COMMIT_SHORT}
                Branch: ${GIT_BRANCH}
                
                Check: ${BUILD_URL}console
                """
                echo message
                
                // emailext (
                //     subject: "❌ Build Failed: ${PROJECT_NAME} #${BUILD_NUMBER}",
                //     body: message,
                //     to: 'hnjm@example.com'
                // )
            }
        }
        
        always {
            echo '🧹 تنظيف البيئة...'
            cleanWs(
                deleteDirs: true,
                disableDeferredWipeout: true,
                notFailBuild: true,
                patterns: [
                    [pattern: '**/bin/**', type: 'INCLUDE'],
                    [pattern: '**/obj/**', type: 'INCLUDE']
                ]
            )
        }
    }
}